using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class PutMethodProvider : IHttpMethodProvider
    {
        private readonly HttpMethod method = HttpMethod.Put;

        public HttpMethod HttpMethod => method;
    }
}
