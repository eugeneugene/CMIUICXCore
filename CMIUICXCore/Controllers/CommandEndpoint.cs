using CMIUICXCore.ApiEndpoints;
using CMIUICXCore.Code;
using CMIUICXCore.Services;
using CMIUICXCore.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.Controllers
{
    /// <summary>
    /// Defines the <see cref="CommandEndpoint" />.
    /// </summary>
    public class CommandEndpoint : BaseAsyncEndpoint.WithRequest<CmiuCommandBase>.WithResponse<CmiuCallCommandResponse>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncTcpClient _tcpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEndpoint"/> class.
        /// </summary>
        /// <param name="logger">The logger<see cref="ILogger{EventEndpoint}"/>.</param>
        /// <param name="configuration">The configuration<see cref="IConfiguration"/>.</param>
        /// <param name="tcpClient">The tcpClient<see cref="AsyncTcpClient"/>.</param>
        public CommandEndpoint(ILogger<CommandEndpoint> logger, IConfiguration configuration, AsyncTcpClient tcpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _tcpClient = tcpClient;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
        }

        /// <summary>
        /// The EventAsync.
        /// </summary>
        /// <param name="request">The request<see cref="CmiuCommandBase"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ActionResult}"/>.</returns>
        [Route("icx/[controller]")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        [ProducesResponseType(typeof(CmiuCallCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public override async Task<ActionResult<CmiuCallCommandResponse>> HandleAsync(CmiuCommandBase request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                _logger.LogDebug("CmiuCommandBase: {0}", request);

                if (_configuration.GetValue<bool>("CMIUICXCore:DisableIcx"))
                {
                    _logger.LogInformation("ICX is Disabled");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                if (request is CmiuCallCommand callCommand)
                    return new ActionResult<CmiuCallCommandResponse>(await CallCommandAsync(callCommand));
                else
                    return new NotFoundResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// The CallCommand.
        /// </summary>
        /// <param name="callCommand">The callCommand<see cref="CmiuCallCommand"/>.</param>
        /// <returns>The <see cref="Task{CmiuCallCommandResponse}"/>.</returns>
        [NonAction]
        private async Task<CmiuCallCommandResponse> CallCommandAsync(CmiuCallCommand callCommand)
        {
            byte[] data;
            try
            {
                if (callCommand.CallTypeTag == "CALL_UP")
                    data = IcxHelper.MakeConnectSeq(callCommand.CallNumber1, callCommand.CallNumber2);
                else if (callCommand.CallTypeTag == "CALL_DOWN")
                    data = IcxHelper.MakeDisconnectSeq(callCommand.CallNumber1);
                else
                    throw new NotSupportedException("Unknown type tag: '" + callCommand.CallTypeTag + "'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new CmiuCallCommandResponse(callCommand.Type, DateTime.Now, 1);
            }

            StringBuilder message = new("<Call> '");
            foreach (byte d in data.Skip(2).Take(data.Length - 3))
                message.Append(Convert.ToChar(d));
            message.Append('\'');
            _logger.LogDebug(message.ToString());

            if (_tcpClient != null)
            {
                if (_tcpClient.IsConnected)
                    await _tcpClient.SendAsync(data);
                else
                    _logger.LogWarning("Not connected to ICX");
            }
            return new CmiuCallCommandResponse(callCommand.Type, DateTime.Now, 0);
        }
    }
}
