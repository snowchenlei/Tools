using System.Security.Cryptography;
using System.Text;

namespace Snow.Tools.Encrypt;

public static class HMACSHA256Encrypt
{
    /// <summary>
    /// Base64 SHA256
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>加密值</returns>
    public static string EncryptWithSHA256(string data, string secret)
    {
        var hashData = Encrypt(data, secret);
        return Convert.ToBase64String(hashData);
    }

    /// <summary>
    /// 原始64位 SHA256
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>加密值</returns>
    public static string EncryptWithSHA256Original(string data, string secret)
    {
        var hashData = Encrypt(data, secret);
        return BitConverter.ToString(hashData).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 加密核心算法
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>加密字节</returns>
    private static byte[] Encrypt(string data, string secret)
    {
        Check.NotNullOrEmpty(data, nameof(data));
        Check.NotNullOrEmpty(secret, nameof(secret));

        var encoding = Encoding.UTF8;
        var keyByte = encoding.GetBytes(secret);
        var dataBytes = encoding.GetBytes(data);
        using var hmac256 = new HMACSHA256(keyByte);

        var hashData = hmac256.ComputeHash(dataBytes);
        return hashData;
    }
}