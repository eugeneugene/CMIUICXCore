using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public partial class HttpRequestFactory<TNamedHttpClientProvider>
    {
        /// <summary>
        /// private HttpRequest
        /// </summary>
        private class HttpRequest : IHttpRequest
        {
            private readonly HttpRequestMessage _requestMessage;
            private readonly INamedHttpClientProvider _namedHttpClient;

            public HttpRequest(IHttpMethodProvider methodProvider, IUriExtension uri, INamedHttpClientProvider namedHttpClient)
            {
                if (uri == null)
                    throw new ArgumentNullException(nameof(uri));
                if (methodProvider == null)
                    throw new ArgumentNullException(nameof(methodProvider));

                _namedHttpClient = namedHttpClient ?? throw new ArgumentNullException(nameof(namedHttpClient));
                _requestMessage = new HttpRequestMessage(methodProvider.HttpMethod, uri.Uri);
            }

            public HttpRequest(IHttpMethodProvider methodProvider, string uri, INamedHttpClientProvider namedHttpClient)
            {
                if (uri == null)
                    throw new ArgumentNullException(nameof(uri));
                if (methodProvider == null)
                    throw new ArgumentNullException(nameof(methodProvider));

                _namedHttpClient = namedHttpClient ?? throw new ArgumentNullException(nameof(namedHttpClient));
                _requestMessage = new HttpRequestMessage(methodProvider.HttpMethod, uri);
            }

            public HttpRequest(IHttpMethodProvider methodProvider, Uri uri, INamedHttpClientProvider namedHttpClient)
            {
                if (uri == null)
                    throw new ArgumentNullException(nameof(uri));
                if (methodProvider == null)
                    throw new ArgumentNullException(nameof(methodProvider));

                _namedHttpClient = namedHttpClient ?? throw new ArgumentNullException(nameof(namedHttpClient));
                _requestMessage = new HttpRequestMessage(methodProvider.HttpMethod, uri);
            }

            public IHttpRequest HandleRequest(IRequestHandler requestHandler)
            {
                if (requestHandler == null)
                    throw new ArgumentNullException(nameof(requestHandler));

                requestHandler.Handle(_requestMessage);
                return this;
            }

            public async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken)
            {
                var client = _namedHttpClient.CreateClient();
                var response = await client.SendAsync(_requestMessage, cancellationToken);
                return response;
            }
        }
    }
}