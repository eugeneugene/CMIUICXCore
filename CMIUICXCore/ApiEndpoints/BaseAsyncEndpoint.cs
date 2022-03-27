using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace CMIUICXCore.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// Thanks to ardalis: https://github.com/ardalis/ApiEndpoints
    /// </summary>
    public static class BaseAsyncEndpoint
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResponse<TResponse> : BaseEndpointAsync
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResponse : BaseEndpointAsync
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithRequest<TRequest1, TRequest2>
        {
            public abstract class WithResponse<TResponse> : BaseEndpointAsync
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResponse : BaseEndpointAsync
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithRequest<TRequest1, TRequest2, TRequest3>
        {
            public abstract class WithResponse<TResponse> : BaseEndpointAsync
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    TRequest3 request3,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResponse : BaseEndpointAsync
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    TRequest3 request3,
                    CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithRequest<TRequest1, TRequest2, TRequest3, TRequest4>
        {
            public abstract class WithResponse<TResponse> : BaseEndpointAsync
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    TRequest3 request3,
                    TRequest4 request4,
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResponse : BaseEndpointAsync
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest1 request1,
                    TRequest2 request2,
                    TRequest3 request3,
                    TRequest4 request4,
                    CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResponse<TResponse> : BaseEndpointAsync
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            public abstract class WithoutResponse : BaseEndpointAsync
            {
                public abstract Task<ActionResult> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }
        }
    }

    /// <summary>
    /// A base class for all asynchronous endpoints.
    /// </summary>
    [ApiController]
    public abstract class BaseEndpointAsync : ControllerBase
    {
    }
}
