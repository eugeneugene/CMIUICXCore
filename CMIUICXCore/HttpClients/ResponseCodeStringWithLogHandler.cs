using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseCodeStringWithLogHandler : IResponseHandler
    {
        private readonly ILogger _logger;
        private readonly HttpStatusCode _code;
        private readonly Action<string> _action;

        public ResponseCodeStringWithLogHandler(ILogger logger, HttpStatusCode code, Action<string> action)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _code = code;
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            _logger.LogTrace("Content:");
            _logger.LogTrace(responseContent);

            if (response.StatusCode == _code)
                _action.Invoke(responseContent);

            return response;
        }
    }
}
