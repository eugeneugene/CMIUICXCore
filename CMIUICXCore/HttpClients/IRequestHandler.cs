using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public interface IRequestHandler
    {
        HttpRequestMessage Handle(HttpRequestMessage request);
    }
}
