using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public interface IResponseHandler
    {
        HttpResponseMessage Handle(HttpResponseMessage response);
    }
}