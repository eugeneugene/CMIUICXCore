using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class PostMethodProvider : IHttpMethodProvider
    {
        private readonly HttpMethod method = HttpMethod.Post;

        public HttpMethod HttpMethod => method;
    }
}
