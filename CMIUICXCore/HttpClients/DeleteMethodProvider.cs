using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class DeleteMethodProvider : IHttpMethodProvider
    {
        private readonly HttpMethod method = HttpMethod.Delete;

        public HttpMethod HttpMethod => method;
    }
}
