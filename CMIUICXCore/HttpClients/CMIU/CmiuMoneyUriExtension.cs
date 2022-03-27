using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuMoneyUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuMoneyUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/money");
        }
    }
}
