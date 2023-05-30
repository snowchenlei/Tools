using System.Security.Cryptography;
using System.Text;

namespace Snow.Tools.Security.Cryptography.Hmacs;

/// <summary>
/// HMAC加密
/// </summary>
public sealed class HMACEncrypt : IDisposable
{
    private readonly HMAC _hmac;

    public HMACEncrypt(HMACMode mode)
    {
        _hmac = HMACFactory.CreateHmac(mode);
    }

    /// <summary>
    /// Base64
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>Base64加密数据</returns>
    public string EncryptToBase64(string data, string secret)
    {
        var hashData = Encrypt(data, secret);
        return Convert.ToBase64String(hashData);
    }

    /// <summary>
    /// 16进制
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="secret">密钥</param>
    /// <returns>16进制加密数据</returns>
    public string EncryptToHex(string data, string secret)
    {
        var hashData = Encrypt(data, secret);
        return BitConverter.ToString(hashData).Replace("-", "").ToLower();
    }

    public byte[] Encrypt(string data, string secret)
    {
        Check.NotNullOrEmpty(data, nameof(data));
        Check.NotNullOrEmpty(secret, nameof(secret));

        var encoding = Encoding.UTF8;
        var keyByte = encoding.GetBytes(secret);
        var dataBytes = encoding.GetBytes(data);

        _hmac.Key = keyByte;

        return _hmac.ComputeHash(dataBytes);
    }

    public void Dispose()
    {
        _hmac.Dispose();
    }
}