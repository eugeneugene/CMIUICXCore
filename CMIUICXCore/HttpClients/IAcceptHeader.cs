using System.Net.Http.Headers;

namespace SBCommon.HttpClients.New
{
    public interface IAcceptHeader
    {
        MediaTypeWithQualityHeaderValue Header { get; }
    }
}
