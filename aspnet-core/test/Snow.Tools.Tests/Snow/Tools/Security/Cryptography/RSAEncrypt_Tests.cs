using System.Text;
using Snow.Tools.Security.Cryptography.Rsas;
using Xunit;

namespace Snow.Tools.Tests.Snow.Tools.Security.Cryptography;

public class RSAEncrypt_Tests
{
    private string _privateKey = "";
    private string _publicKey = "";

    [Fact]
    public void Sign_Test()
    {
        var rsa = new RSAEncrypt(RSAType.RSA2, Encoding.UTF8, _privateKey, _publicKey);
        var result = rsa.Sign("111");
    }
}