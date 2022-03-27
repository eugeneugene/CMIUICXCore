using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseCodeWithLogHandler<T> : IResponseHandler where T : class
    {
        private readonly ILogger _logger;
        private readonly HttpStatusCode _code;
        private readonly Action<T> _action;

        public ResponseCodeWithLogHandler(ILogger logger, HttpStatusCode code, Action<T> action)
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
            {
                var content = JsonSerializer.Deserialize<T>(responseContent, null);
                _action.Invoke(content);
            }

            return response;
        }
    }
}
