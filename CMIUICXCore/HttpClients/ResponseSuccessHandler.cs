using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseSuccessHandler<T> : IResponseHandler where T : class
    {
        private readonly Action<T> _action;

        public ResponseSuccessHandler(Action<T> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var content = JsonSerializer.Deserialize<T>(responseContent, null);
                _action.Invoke(content);
            }

            return response;
        }
    }

    public class ResponseSuccessHandler : IResponseHandler
    {
        private readonly Action _action;

        public ResponseSuccessHandler(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
                _action.Invoke();

            return response;
        }
    }
}
