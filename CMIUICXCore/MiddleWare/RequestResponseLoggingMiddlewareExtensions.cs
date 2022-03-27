using Microsoft.AspNetCore.Builder;

namespace CMIUICXCore.MiddleWare
{
    /// <summary>
    /// Middleware логирование данных http запросов и ответов (расширение)
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Расширение, чтобы включить логирование
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
