using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuCallEventUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuCallEventUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/call_event");
        }
    }
}
