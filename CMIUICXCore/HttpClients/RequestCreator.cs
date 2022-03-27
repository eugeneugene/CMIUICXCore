using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class RequestCreator
    {
        private readonly IHttpMethodProvider _methodProvider;
        private readonly Uri _uri;

        public RequestCreator(IHttpMethodProvider methodProvider, IUriExtension uri)
        {
            _methodProvider = methodProvider;
            _uri = uri.Uri;
        }

        public HttpRequestMessage Create()
        {
            return new HttpRequestMessage(_methodProvider.HttpMethod, _uri);
        }
    }
}
