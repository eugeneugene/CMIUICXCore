using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SBCommon.HttpClients.New
{
    public class RequestJsonBodyWithLogDecorator<TBody> : IRequestHandler where TBody : class
    {
        private readonly ILogger _logger;
        private readonly TBody _body;

        public RequestJsonBodyWithLogDecorator(ILogger logger, TBody body)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(_body);
            string requestContent = Encoding.UTF8.GetString(jsonUtf8Bytes);

            _logger.LogTrace("Content:");
            _logger.LogTrace(requestContent);

            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
            return request;
        }
    }
}
