using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseSuccessStringWithLogHandler : IResponseHandler
    {
        private readonly ILogger _logger;
        private readonly Action<string> _action;

        public ResponseSuccessStringWithLogHandler(ILogger logger, Action<string> action)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            _logger.LogTrace("Content:");
            _logger.LogTrace(responseContent);

            if (response.IsSuccessStatusCode)
                _action.Invoke(responseContent);

            return response;
        }
    }
}
