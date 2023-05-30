using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Snow.Tools.Security.Cryptography.Rsas;

public sealed class RSAEncrypt : IDisposable
{
    private readonly RSA _privateRsa;
    private readonly RSA? _publicRsa;
    private readonly Encoding _encoding;
    private HashAlgorithmName _hashAlgorithmName;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="privateRsa">私有rsa</param>
    /// <param name="publicRsa">公有rsa</param>
    /// <param name="rsaType">加密算法类型 RSA SHA1;RSA2 SHA256 密钥长度至少为2048</param>
    /// <param name="encoding">编码类型</param>
    public RSAEncrypt(RSAType rsaType, Encoding encoding, RSA privateRsa, RSA? publicRsa = null)
    {
        _privateRsa = privateRsa;
        _publicRsa = publicRsa;
        InitHashAlgorithmName(rsaType);
        _encoding = encoding;
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="rsaType">加密算法类型 RSA SHA1;RSA2 SHA256 密钥长度至少为2048</param>
    /// <param name="encoding">编码类型</param>
    /// <param name="privateKey">私钥</param>
    /// <param name="publicKey">公钥</param>
    public RSAEncrypt(RSAType rsaType, Encoding encoding, string privateKey, string? publicKey = null)
    {
        _encoding = encoding;
        InitHashAlgorithmName(rsaType);
        if (!privateKey.IsNullOrEmpty())
        {
            _privateRsa = CreateRsaFromPrivateKey(privateKey);
        }

        if (!String.IsNullOrEmpty(publicKey))
        {
            _publicRsa = CreateRsaFromPublicKey(publicKey);
        }
    }

    /// <summary>
    /// 初始化加密方式
    /// </summary>
    /// <param name="rsaType"></param>
    private void InitHashAlgorithmName(RSAType rsaType)
    {
        _hashAlgorithmName = rsaType switch
        {
            RSAType.RSA => HashAlgorithmName.SHA1,
            RSAType.RSA2 => HashAlgorithmName.SHA256,
            _ => HashAlgorithmName.SHA256
        };
    }

    /// <summary>
    /// 私钥签名
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public string Sign(string content)
    {
        Check.NotNullOrEmpty(content, nameof(content));

        var signBytes = _privateRsa.SignData(_encoding.GetBytes(content),
            _hashAlgorithmName, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signBytes);
    }

    /// <summary>
    /// 公钥验签
    /// </summary>
    /// <param name="sign">签名</param>
    /// <param name="content">内容</param>
    /// <returns></returns>
    public bool Verify(string sign, string content)
    {
        Check.NotNullOrEmpty(sign, nameof(sign));
        Check.NotNullOrEmpty(content, nameof(content));

        if (_publicRsa is null)
        {
            throw new Exception("public rsa can not be null");
        }
        return _publicRsa.VerifyData(_encoding.GetBytes(content), Convert.FromBase64String(sign),
            _hashAlgorithmName, RSASignaturePadding.Pkcs1);
    }

    private RSA CreateRsaFromPrivateKey(string privateKey)
    {
        Check.NotNullOrEmpty(privateKey, nameof(privateKey));

        var keyBytes = (ReadOnlySpan<byte>)Convert.FromBase64String(privateKey);

        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(keyBytes, out _);
        return rsa;
    }

    private RSA CreateRsaFromPublicKey(string publicKey)
    {
        Check.NotNullOrEmpty(publicKey, nameof(publicKey));

        var keyBytes = Convert.FromBase64String(publicKey);

        var rsa = RSA.Create();
        var rsaParameters = PublicKeyFactory.CreateKey(keyBytes) as RsaKeyParameters;
        if (rsaParameters is null)
        {
            throw new ArgumentException("not support publicKey", nameof(publicKey));
        }
        rsa.ImportParameters(new RSAParameters()
        {
            Modulus = rsaParameters.Modulus.ToByteArrayUnsigned(),
            Exponent = rsaParameters.Exponent.ToByteArrayUnsigned()
        });
        return rsa;
    }

    public void Dispose()
    {
        _privateRsa.Dispose();
        _publicRsa?.Dispose();
    }
}