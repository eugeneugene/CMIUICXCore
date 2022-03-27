using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public interface INamedHttpClientProvider
    {
        string ClientName { get; }
        HttpClient CreateClient();
    }
}
