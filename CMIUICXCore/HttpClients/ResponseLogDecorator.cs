using CMIUICXCore.Code;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class ResponseLogDecorator : IResponseHandler
    {
        private readonly ILogger _logger;

        public ResponseLogDecorator(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            _logger.LogTrace("Response:");
            _logger.LogTrace("StatusCode: {0}, ReasonPhrase: '{1}', Version: {2}, Headers: {3}", response.StatusCode, response.ReasonPhrase ?? "<null>", response.Version,
                HeaderUtilities.DumpHeaders(response.Headers));

            return response;
        }
    }
}