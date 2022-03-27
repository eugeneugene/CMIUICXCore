using System;

namespace SBCommon.HttpClients.New
{
    public class CmiuUploadImageUriExtension : IUriExtension
    {
        private readonly Uri _uri;

        public Uri Uri => _uri;

        public CmiuUploadImageUriExtension(IUriProvider uriProvider)
        {
            _uri = new Uri(uriProvider.Uri, "/WebService/webresources/service/upload_image");
        }
    }
}
