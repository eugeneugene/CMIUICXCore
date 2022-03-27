using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class CmiuHttpClientProvider : INamedHttpClientProvider
    {
        public const string HttpClientName = "CmiuHttpClient";

        private readonly IHttpClientFactory _clientFactory;

        public string ClientName => HttpClientName;

        public HttpClient CreateClient()
        {
            return _clientFactory.CreateClient(ClientName);
        }

        public CmiuHttpClientProvider(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }
    }
}
