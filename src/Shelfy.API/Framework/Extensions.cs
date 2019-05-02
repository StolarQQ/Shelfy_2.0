using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace Shelfy.API.Framework
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMyExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();

        public static IApplicationBuilder RegisterSerilog(this IApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich
                .FromLogContext()
                .MinimumLevel.Warning()
                .WriteTo.File("Logs/logs.txt")
                .CreateLogger();

            return builder;
        }

    }
}
