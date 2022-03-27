using CMIUICXCore.Code;
using System;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace SBCommon.HttpClients.New
{
    public class RequestXmlBodyDecorator<TBody> : IRequestHandler where TBody : class
    {
        private readonly TBody _body;

        public RequestXmlBodyDecorator(TBody body)
        {
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
            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/xml");
            return request;
        }
    }
}
