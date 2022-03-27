using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuMovementUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuMovementUriExtension(IUriProvider uriProvider, string movementType)
        {
            _uri = new Uri(uriProvider.Uri, $"/WebService/webresources/service/{movementType}");
        }
    }
}
