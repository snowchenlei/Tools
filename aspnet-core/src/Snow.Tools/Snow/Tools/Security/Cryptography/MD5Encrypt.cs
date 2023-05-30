using System.Security.Cryptography;
using System.Text;

namespace Snow.Tools.Security.Cryptography;

/// <summary>
/// MD5加密
/// </summary>
public static class MD5Encrypt
{
    /// <summary>
    /// 使用UTF-8加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <returns>密文</returns>
    public static byte[] Encrypt(string data)
    {
        Check.NotNullOrEmpty(data, nameof(data));

        return Encrypt(data, Encoding.UTF8);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <returns>密文</returns>
    public static byte[] Encrypt(byte[] data)
    {
        Check.NotNullOrEmpty(data, nameof(data));

        using var md5 = CreateMd5();
        return md5.ComputeHash(data);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <param name="encoding">编码</param>
    /// <returns>密文</returns>
    public static byte[] Encrypt(string data, Encoding encoding)
    {
        Check.NotNullOrEmpty(data, nameof(data));

        using var md5 = CreateMd5();
        var dataBytes = encoding.GetBytes(data);
        return md5.ComputeHash(dataBytes);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <returns>16位密文</returns>
    public static string EncryptTo16(string data)
    {
        var result = Encrypt(data);
        return Convert.ToHexString(result, 4, 8);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <returns>32位密文</returns>
    public static string EncryptTo32(string data)
    {
        var hashBytes = Encrypt(data);
        return Convert.ToHexString(hashBytes);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">待加密数据</param>
    /// <returns>64位密文</returns>
    public static string EncryptToBase64(string data)
    {
        var result = Encrypt(data);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// 创建MD5对象
    /// </summary>
    /// <returns></returns>
    private static MD5 CreateMd5()
    {
        return MD5.Create();
    }
}