using CMIUICXCore.Code;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace SBCommon.HttpClients.New
{
    public class RequestLogDecorator : IRequestHandler
    {
        private readonly ILogger _logger;

        public RequestLogDecorator(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _logger.LogTrace("Request:");
            _logger.LogTrace("Method: {0}, RequestUri: {1}, Version: {2}, Headers: {3}",
                request.Method, request.RequestUri?.ToString() ?? "<null>", request.Version, HeaderUtilities.DumpHeaders(request.Headers));

            return request;
        }
    }
}