using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class RequestContentDecorator : IRequestHandler
    {
        private readonly HttpContent _content;

        public RequestContentDecorator(HttpContent content)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Content = _content;
            return request;
        }
    }
}
