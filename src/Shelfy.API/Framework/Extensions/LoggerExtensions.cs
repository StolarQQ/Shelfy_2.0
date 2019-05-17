using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Shelfy.API.Framework.Extensions
{
    public static class LoggerExtensions
    {
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