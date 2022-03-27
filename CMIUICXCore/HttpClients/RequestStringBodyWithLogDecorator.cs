using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;

namespace SBCommon.HttpClients.New
{
    public class RequestStringBodyWithLogDecorator : IRequestHandler
    {
        private readonly ILogger _logger;
        private readonly string _body;
        private readonly Encoding _encoding;
        private readonly string _mediaType;

        public RequestStringBodyWithLogDecorator(ILogger logger, string body, Encoding encoding, string mediaType)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _body = body ?? throw new ArgumentNullException(nameof(body)); ;
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding)); ;
            _mediaType = mediaType ?? throw new ArgumentNullException(nameof(mediaType)); ;
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _logger.LogTrace("Content:");
            _logger.LogTrace(_body);

            request.Content = new StringContent(_body, _encoding, _mediaType);
            return request;
        }
    }
}
