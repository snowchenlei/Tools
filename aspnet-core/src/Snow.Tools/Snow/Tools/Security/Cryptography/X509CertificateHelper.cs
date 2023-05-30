using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Snow.Tools.Security.Cryptography;

public class X509CertificateHelper
{
    private RSA? CreateRsaFromPrivate(string path, string password)
    {
        var cert = new X509Certificate2(path, password);
        return cert.GetRSAPrivateKey();
    }

    private RSA? CreateRsaFromPublic(string path, string password)
    {
        var cert = new X509Certificate2(path, password);
        return cert.GetRSAPublicKey();
    }
}