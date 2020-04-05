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
                .MinimumLevel.Information()
                .WriteTo.File("Logs/logs.txt") // Just for testing purpose
                .CreateLogger();

            return builder;
        }
    }
}