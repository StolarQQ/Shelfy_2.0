using Microsoft.AspNetCore.Builder;

namespace Shelfy.API.Framework.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
