using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuEventsUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuEventsUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/events");
        }
    }
}
