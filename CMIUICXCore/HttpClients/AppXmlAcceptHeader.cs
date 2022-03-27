using System.Net.Http.Headers;

namespace SBCommon.HttpClients.New
{
    public class AppXmlAcceptHeader : IAcceptHeader
    {
        private readonly MediaTypeWithQualityHeaderValue _header = new("application/xml");
        public MediaTypeWithQualityHeaderValue Header => _header;
    }
}
