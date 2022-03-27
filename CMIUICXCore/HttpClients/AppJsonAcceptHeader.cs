using System.Net.Http.Headers;

namespace SBCommon.HttpClients.New
{
    public class AppJsonAcceptHeader : IAcceptHeader
    {
        private readonly MediaTypeWithQualityHeaderValue _header = new("application/json");
        public MediaTypeWithQualityHeaderValue Header => _header;
    }
}
