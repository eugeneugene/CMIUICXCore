using Microsoft.Extensions.Logging;
using System;

namespace SBCommon.HttpClients.New
{
    /// <summary>
    /// DI class
    /// Visibility: Transient
    /// </summary>
    public partial class HttpRequestFactory<TNamedHttpClientProvider> : IHttpRequestFactory where TNamedHttpClientProvider : INamedHttpClientProvider
    {
        private readonly ILogger _logger;
        private readonly TNamedHttpClientProvider _namedHttpClient;

        public HttpRequestFactory(ILogger<HttpRequestFactory<TNamedHttpClientProvider>> logger, TNamedHttpClientProvider namedHttpClient)
        {
            _logger = logger;
            _namedHttpClient = namedHttpClient;
            _logger.LogTrace("Создание экземпляра {0}", GetType().Name);
            _logger.LogTrace("Named Http Client {0}", _namedHttpClient.ClientName);
        }

        public IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, IUriExtension uri)
        {
            return new HttpRequest(methodProvider, uri, _namedHttpClient);
        }

        public IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, string uri)
        {
            return new HttpRequest(methodProvider, uri, _namedHttpClient);
        }

        public IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, Uri uri)
        {
            return new HttpRequest(methodProvider, uri, _namedHttpClient);
        }
    }
}
