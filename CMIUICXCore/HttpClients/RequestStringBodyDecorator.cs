using System;
using System.Net.Http;
using System.Text;

namespace SBCommon.HttpClients.New
{
    public class RequestStringBodyDecorator : IRequestHandler
    {
        private readonly string _body;
        private readonly Encoding _encoding;
        private readonly string _mediaType;

        public RequestStringBodyDecorator(string body, Encoding encoding, string mediaType)
        {
            _body = body ?? throw new ArgumentNullException(nameof(body)); ;
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding)); ;
            _mediaType = mediaType ?? throw new ArgumentNullException(nameof(mediaType)); ;
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Content = new StringContent(_body, _encoding, _mediaType);
            return request;
        }
    }
}
