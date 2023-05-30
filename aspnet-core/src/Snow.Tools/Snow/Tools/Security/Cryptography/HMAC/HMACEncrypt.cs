using System.Text;

namespace Snow.Tools.Security.Cryptography.HMAC;

/// <summary>
/// HMAC加密
/// </summary>
public static class HMACEncrypt
{
    /// <summary>
    /// Base64
    /// </summary>
    /// <param name="mode">密钥模式</param>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>Base64加密数据</returns>
    public static string EncryptToBase64(HMACMode mode, string data, string secret)
    {
        var hashData = Encrypt(mode, data, secret);
        return Convert.ToBase64String(hashData);
    }

    /// <summary>
    /// 16进制
    /// </summary>
    /// <param name="mode">密钥模式</param>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>16进制加密数据</returns>
    public static string EncryptToHex(HMACMode mode, string data, string secret)
    {
        var hashData = Encrypt(mode, data, secret);
        return BitConverter.ToString(hashData).Replace("-", "").ToLower();
    }

    public static byte[] Encrypt(HMACMode mode, string data, string secret)
    {
        Check.NotNullOrEmpty(data, nameof(data));
        Check.NotNullOrEmpty(secret, nameof(secret));

        var encoding = Encoding.UTF8;
        var keyByte = encoding.GetBytes(secret);
        var dataBytes = encoding.GetBytes(data);
        using var hmac = HMACFactory.CreateHmac(mode);
        hmac.Key = keyByte;

        return hmac.ComputeHash(dataBytes);
    }
}