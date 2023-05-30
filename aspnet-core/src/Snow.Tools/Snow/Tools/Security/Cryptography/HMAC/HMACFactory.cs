using System.Security.Cryptography;

namespace Snow.Tools.Security.Cryptography.HMAC;

/// <summary>
/// HMAC工厂
/// </summary>
// ReSharper disable once InconsistentNaming
public class HMACFactory
{
    public static System.Security.Cryptography.HMAC CreateHmac(HMACMode mode)
    {
        return mode switch
        {
            HMACMode.MD5 => new HMACMD5(),
            HMACMode.SHA1 => new HMACSHA1(),
            HMACMode.SHA256 => new HMACSHA256(),
            HMACMode.SHA384 => new HMACSHA384(),
            HMACMode.SHA512 => new HMACSHA512(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}