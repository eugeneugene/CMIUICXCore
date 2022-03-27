using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


namespace SBCommon.HttpClients.New
{
    public interface IHttpRequest
    {
        IHttpRequest HandleRequest(IRequestHandler requestHandler);
        Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken);
    }
}
