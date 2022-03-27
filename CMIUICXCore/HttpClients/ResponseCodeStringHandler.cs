using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SBCommon.HttpClients.New
{
    public class ResponseCodeStringHandler : IResponseHandler
    {
        private readonly HttpStatusCode _code;
        private readonly Action<string> _action;

        public ResponseCodeStringHandler(HttpStatusCode code, Action<string> action)
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
                _action.Invoke(responseContent);

            return response;
        }
    }
}
