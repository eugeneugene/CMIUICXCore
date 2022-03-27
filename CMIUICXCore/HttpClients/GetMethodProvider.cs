using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class GetMethodProvider : IHttpMethodProvider
    {
        private readonly HttpMethod method = HttpMethod.Get;

        public HttpMethod HttpMethod => method;
    }
}
