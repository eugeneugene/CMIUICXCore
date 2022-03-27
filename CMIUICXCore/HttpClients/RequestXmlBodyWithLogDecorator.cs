using CMIUICXCore.Code;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace SBCommon.HttpClients.New
{
    public class RequestXmlBodyWithLogDecorator<TBody> : IRequestHandler where TBody : class
    {
        private readonly ILogger _logger;
        private readonly TBody _body;

        public RequestXmlBodyWithLogDecorator(ILogger logger, TBody body)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _body = body ?? throw new ArgumentNullException(nameof(body)); ;
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var serializer = new XmlSerializer(_body.GetType());
            using var writer = new Utf8StringWriter();
            serializer.Serialize(writer, _body);
            var requestContent = writer.ToString();

            _logger.LogTrace("Content:");
            _logger.LogTrace(requestContent);

            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/xml");
            return request;
        }
    }
}
