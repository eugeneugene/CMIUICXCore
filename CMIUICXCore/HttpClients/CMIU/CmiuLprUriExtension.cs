using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuLprUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuLprUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/lpr");
        }
    }
}
