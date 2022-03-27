using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuLevelsUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuLevelsUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/levels");
        }
    }
}
