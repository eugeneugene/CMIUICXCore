using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public interface IHttpMethodProvider
    {
        HttpMethod HttpMethod { get; }
    }
}
