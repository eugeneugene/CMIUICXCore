using CMIUICXCore.Services.HttpClients;
using CMIUICXCore.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Services
{
    public class IcxHandlingService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICmiuHttpClient _cmiuHttpClient;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly AsyncTcpClient _tcpClient;

        private readonly List<byte> ReceiveBuffer = new();

        public IcxHandlingService(ILogger<IcxHandlingService> logger, IConfiguration configuration, ICmiuHttpClient cmiuHttpClient,
            IHostApplicationLifetime appLifetime, AsyncTcpClient tcpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _cmiuHttpClient = cmiuHttpClient;
            _appLifetime = appLifetime;
            _tcpClient = tcpClient;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Запуск {0}", GetType().Name);

            try
            {
                if (_configuration.GetValue<bool>("CMIUICXCore:DisableIcx"))
                {
                    _logger.LogInformation("ICX is Disabled");
                    return;
                }

                var IcxAddress = _configuration.GetValue("CMIUICXCore:IcxAddress", string.Empty);
                var IcxPort = _configuration.GetValue("CMIUICXCore:IcxPort", -1);

                if (IcxAddress.Length == 0)
                {
                    _logger.LogError("Не задан IcxAddress");
                    _appLifetime.StopApplication();
                    return;
                }
                if (IcxPort == -1)
                {
                    _logger.LogError("Не задан IcxPort");
                    _appLifetime.StopApplication();
                    return;
                }

                _tcpClient.OnMessage += OnMessage;
                _tcpClient.OnDataReceived += async (o, a) => await OnDataReceived(o, a);
                _tcpClient.OnDisconnected += OnDisconected;

                while (!_appLifetime.ApplicationStopping.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Connecting...");
                        await _tcpClient.ConnectAsync(IcxAddress, IcxPort, false, _appLifetime.ApplicationStopping);
                        await _tcpClient.ReceiveAsync(_appLifetime.ApplicationStopping);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.ToString());
                        _logger.LogError("Retrying...");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private void OnMessage(object _ob, string Message)
        {
            _logger.LogDebug(Message);
        }

        private async Task OnDataReceived(object _, byte[] message)
        {
            ReceiveBuffer.AddRange(message);
            while (true)
            {
                try
                {
                    int index = ReceiveBuffer.FindIndex(item => item == 0xFF);
                    if (index == -1)
                        break;
                    var _msg = new List<byte>(ReceiveBuffer.Take(index + 1));
                    ReceiveBuffer.RemoveRange(0, index + 1);
                    byte Indicator = _msg[0];
                    CallEventSender sender = null;
                    switch (Indicator)
                    {
                        case 0xF1:
                            _logger.LogTrace("<Idle>");
                            sender = new CallEventSender("call_event", CallTypeTag.Idle, string.Empty, string.Empty, DateTime.Now, 0);
                            break;
                        case 0xF2:
                            {
                                var Load = _msg.GetRange(2, _msg.Count - 3);
                                var msg = ICXMsgFactory.GetICXMessage(Load.ToArray());
                                if (msg != default)
                                {
                                    _logger.LogTrace("<Data> '{0}' {1}", Encoding.UTF8.GetString(Load.ToArray()), msg.ToString());
                                    sender = msg.GetCallEventSender();
                                }
                                else
                                    _logger.LogTrace("<Data> '{0}'", Encoding.UTF8.GetString(Load.ToArray()));
                            }
                            break;
                        case 0xF3:
                            _logger.LogTrace("<Login>");
                            break;
                        case 0xF4:
                            _logger.LogTrace("<Challenge>");
                            break;
                        case 0xF5:
                            _logger.LogTrace("<Response>");
                            break;
                        default:
                            _logger.LogWarning("Unknow message type: {0:X2}", Indicator);
                            break;
                    }
                    if (null != sender)
                    {
                        _logger.LogDebug("Request: {0}", sender);
                        var response = await _cmiuHttpClient.PostCallEventAsync(sender, _appLifetime.ApplicationStopping);
                        _logger.LogDebug("Response: {0}", response);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
        }

        private void OnDisconected(object _ob, EventArgs _args)
        {
            _logger.LogDebug("Disconnected");
        }
    }
}
