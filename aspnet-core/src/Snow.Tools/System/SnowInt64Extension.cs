using System.Runtime.CompilerServices;

namespace System;

public static class SnowInt64Extension
{
    /// <summary>
    /// 时间戳转本地时间
    /// </summary>
    /// <param name="milliseconds">毫秒</param>
    /// <returns>时间</returns>
    public static DateTime ToLocalDateTimeFromUnixTimeMilliseconds(this long milliseconds)
    {
        var dto = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        return dto.ToLocalTime().DateTime;
    }

    /// <summary>
    /// 时间戳转本地时间
    /// </summary>
    /// <param name="seconds">秒</param>
    /// <returns>时间</returns>
    public static DateTime ToLocalDateTimeFromUnixTimeSeconds(this long seconds)
    {
        var dto = DateTimeOffset.FromUnixTimeSeconds(seconds);
        return dto.ToLocalTime().DateTime;
    }
}