using System;

namespace Shelfy.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// The timestamps in JWT are UNIX timestamps counting from 01.01.1970 00:00
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = dateTime.Subtract(new TimeSpan(epoch.Ticks));

            return time.Ticks / 10000;
        }
    }
}