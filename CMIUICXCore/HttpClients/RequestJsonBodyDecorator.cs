using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SBCommon.HttpClients.New
{
    public class RequestJsonBodyDecorator<TBody> : IRequestHandler where TBody : class
    {
        private readonly TBody _body;

        public RequestJsonBodyDecorator(TBody body)
        {
            _body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public HttpRequestMessage Handle(HttpRequestMessage request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(_body);
            string requestContent = Encoding.UTF8.GetString(jsonUtf8Bytes);
            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
            return request;
        }
    }
}
