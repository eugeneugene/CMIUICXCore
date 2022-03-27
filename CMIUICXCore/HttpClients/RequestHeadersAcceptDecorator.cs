using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class RequestHeadersAcceptDecorator<TAcceptHeader> : IRequestHandler where TAcceptHeader : IAcceptHeader
    {
        private readonly TAcceptHeader _acceptHeader;

        public RequestHeadersAcceptDecorator(TAcceptHeader acceptHeader)
        {
            _acceptHeader = acceptHeader ?? throw new ArgumentNullException(nameof(acceptHeader));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Headers.Accept.Add(_acceptHeader.Header);
            return request;
        }
    }
}