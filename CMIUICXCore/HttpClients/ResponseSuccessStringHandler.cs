using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseSuccessStringHandler : IResponseHandler
    {
        private readonly Action<string> _action;

        public ResponseSuccessStringHandler(Action<string> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public HttpResponseMessage Handle(HttpResponseMessage response)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));

            var responseContent = Task.Run(async () => await response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
                _action.Invoke(responseContent);

            return response;
        }
    }
}
