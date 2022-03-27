using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseCodeHandler<T> : IResponseHandler where T : class
    {
        private readonly HttpStatusCode _code;
        private readonly Action<T> _action;

        public ResponseCodeHandler(HttpStatusCode code, Action<T> action)
        {
            _code = code;
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            if (response.StatusCode == _code)
            {
                var content = JsonSerializer.Deserialize<T>(responseContent, null);
                _action.Invoke(content);
            }

            return response;
        }
    }
}
