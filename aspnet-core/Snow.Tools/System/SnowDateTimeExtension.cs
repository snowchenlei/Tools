namespace System;

public static class SnowDateTimeExtension
{
    /// <summary>
    /// 转时间戳Unix
    /// </summary>
    public static long ToUnixTimeMilliseconds(this DateTime dt)
    {
        DateTimeOffset dto = new DateTimeOffset(dt);
        return dto.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 转时间戳Unix
    /// </summary>
    public static long ToUnixTimeSeconds(this DateTime dt)
    {
        DateTimeOffset dto = new DateTimeOffset(dt);
        return dto.ToUnixTimeSeconds();
    }
}