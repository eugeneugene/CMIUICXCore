using System;

namespace SBCommon.HttpClients.New
{
    public interface IHttpRequestFactory
    {
        IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, IUriExtension uri);
        IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, string uri);
        IHttpRequest CreateRequest(IHttpMethodProvider methodProvider, Uri uri);
    }
}
