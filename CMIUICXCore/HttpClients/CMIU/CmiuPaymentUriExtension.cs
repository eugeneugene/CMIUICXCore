using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuPaymentUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuPaymentUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/payments");
        }
    }
}
