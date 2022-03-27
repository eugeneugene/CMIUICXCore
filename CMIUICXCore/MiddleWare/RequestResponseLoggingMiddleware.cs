using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMIUICXCore.MiddleWare
{
    /// <summary>
    /// Middleware логирование данных http запросов и ответов
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new();

        /// <summary>
        /// RequestResponseLoggingMiddleware.ctor
        /// </summary>
        /// <param name="next">RequestDelegate</param>
        /// <param name="logger">ILogger</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            RequestResponseLog _log = new()
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.ToString(),
                RequestedOn = DateTime.Now
            };

            switch (context.Request.Method)
            {
                case "DELETE":
                case "PATCH":
                case "POST":
                case "PUT":
                    context.Request.EnableBuffering();
                    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    _log.Payload = body;
                    break;
            }

            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            try
            {
                // write the response to the log object
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                _log.Response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                _log.StatusCode = context.Response.StatusCode;
                _log.IsSuccessStatusCode = context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299;
                _log.RespondedOn = DateTime.Now;
                _logger.LogTrace(_log.ToString());
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
