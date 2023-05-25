using System.Collections.Generic;
using System.Text;

namespace Snow.Tools
{
    public static class TimesHelper
    {
        /// <summary>
        /// 时间戳转本地时间
        /// </summary>
        /// <param name="milliseconds">毫秒</param>
        /// <returns>时间</returns>
        public static DateTime MillisecondsToLocalTimeDate(long milliseconds)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        /// 时间戳转本地时间
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns>时间</returns>
        public static DateTime SecondsToLocalTimeDate(long seconds)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(seconds);
            return dto.ToLocalTime().DateTime;
        }
    }
}