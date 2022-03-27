using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class HttpResponse : IHttpResponse
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponse(HttpResponseMessage httpResponseMessage)
        {
            _responseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
        }

        public HttpResponse HandleResponse(IResponseHandler responseHandler)
        {
            if (responseHandler == null)
                throw new ArgumentNullException(nameof(responseHandler));

            responseHandler.Handle(_responseMessage);
            return this;
        }
    }
}