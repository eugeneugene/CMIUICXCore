using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class RequestHeadersDecorator : IRequestHandler
    {
        public IDictionary<string, string> Headers { get; }

        public RequestHeadersDecorator(IDictionary<string, string> headers)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (Headers != null && Headers.Any())
            {
                foreach (var key in Headers.Keys)
                    request.Headers.Add(key, Headers[key]);
            }
            return request;
        }
    }
}
