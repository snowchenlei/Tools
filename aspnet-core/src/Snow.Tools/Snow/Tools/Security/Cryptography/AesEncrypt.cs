using System.Security.Cryptography;
using System.Text;

namespace Snow.Tools.Security.Cryptography;

public static class AesEncrypt
{
    /// <summary>
    /// 使用AES加密字符串
    /// </summary>
    /// <param name="key">秘钥，需要128位、256位.....</param>
    /// <param name="content">加密内容</param>
    /// <returns>16进制字符串</returns>
    public static string EncryptToHex(string key, string content)
    {
        byte[] resultBytes = Encrypt(key, content, CipherMode.ECB, PaddingMode.PKCS7, Encoding.UTF8);
        return Convert.ToHexString(resultBytes);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="key">秘钥</param>
    /// <param name="content">加密内容</param>
    /// <param name="cipherMode">模式</param>
    /// <param name="paddingMode">填充</param>
    /// <param name="encoding">编码</param>
    /// <returns></returns>
    public static byte[] Encrypt(string key, string content, CipherMode cipherMode, PaddingMode paddingMode, Encoding encoding)
    {
        var keyBytes = Convert.FromBase64String(key);
        byte[] toEncryptArray = encoding.GetBytes(content);

        using var aes = CreateAes(keyBytes, cipherMode, paddingMode);
        var transform = aes.CreateEncryptor();
        return transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
    }

    private static Aes CreateAes(byte[] keyBytes, CipherMode cipherMode, PaddingMode paddingMode)
    {
        var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.Mode = cipherMode;
        aes.Padding = paddingMode;
        return aes;
    }
}