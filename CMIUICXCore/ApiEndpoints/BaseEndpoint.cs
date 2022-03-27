using Microsoft.AspNetCore.Mvc;

namespace CMIUICXCore.ApiEndpoints
{
	/// <summary>
	/// A base class for an endpoint that accepts parameters.
	/// Thanks to ardalis: https://github.com/ardalis/ApiEndpoints
	/// </summary>
	public static class BaseEndpoint
	{
		public static class WithRequest<TRequest>
		{
			public abstract class WithResponse<TResponse> : BaseEndpointSync
			{
				public abstract ActionResult<TResponse> Handle(TRequest request);
			}

			public abstract class WithoutResponse : BaseEndpointSync
			{
				public abstract ActionResult Handle(TRequest request);
			}
		}

		public static class WithRequest<TRequest1, TRequest2>
		{
			public abstract class WithResponse<TResponse> : BaseEndpointSync
			{
				public abstract ActionResult<TResponse> Handle(TRequest1 request1, TRequest2 request2);
			}

			public abstract class WithoutResponse : BaseEndpointSync
			{
				public abstract ActionResult Handle(TRequest1 request1, TRequest2 request2);
			}
		}

		public static class WithRequest<TRequest1, TRequest2, TRequest3>
		{
			public abstract class WithResponse<TResponse> : BaseEndpointSync
			{
				public abstract ActionResult<TResponse> Handle(TRequest1 request1, TRequest2 request2, TRequest3 request3);
			}

			public abstract class WithoutResponse : BaseEndpointSync
			{
				public abstract ActionResult Handle(TRequest1 request1, TRequest2 request2, TRequest3 request3);
			}
		}

		public static class WithRequest<TRequest1, TRequest2, TRequest3, TRequest4>
		{
			public abstract class WithResponse<TResponse> : BaseEndpointSync
			{
				public abstract ActionResult<TResponse> Handle(TRequest1 request1, TRequest2 request2, TRequest3 request3, TRequest4 request4);
			}

			public abstract class WithoutResponse : BaseEndpointSync
			{
				public abstract ActionResult Handle(TRequest1 request1, TRequest2 request2, TRequest3 request3, TRequest4 request4);
			}
		}

		public static class WithoutRequest
		{
			public abstract class WithResponse<TResponse> : BaseEndpointSync
			{
				public abstract ActionResult<TResponse> Handle();
			}

			public abstract class WithoutResponse : BaseEndpointSync
			{
				public abstract ActionResult Handle();
			}
		}
	}

	/// <summary>
	/// A base class for all synchronous endpoints.
	/// </summary>
	[ApiController]
	public abstract class BaseEndpointSync : ControllerBase
	{
	}
}
