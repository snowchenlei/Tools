using Shouldly;
using Snow.Tools.Security.Cryptography.HMAC;
using Xunit;

namespace Snow.Tools.Tests.Snow.Tools.Security.Cryptography;

public class HMACSHA256Encrypt_Tests
{
    [Fact]
    public void EncryptToBase64_Test()
    {
        HMACEncrypt.EncryptToBase64(HMACMode.MD5, "1234", "1111")
            .ShouldBe("qWtbpRZpotbzKm5Nc51RCA==");

        HMACEncrypt.EncryptToBase64(HMACMode.SHA1, "1234", "1111")
            .ShouldBe("vUFQtOGGgrEai05pTN86TDbQqtk=");

        HMACEncrypt.EncryptToBase64(HMACMode.SHA256, "1234", "1111")
            .ShouldBe("OIRyAA0CHB99CJxkcnUM34C5F3Xdf/aaWdMkIWzf8V4=");

        HMACEncrypt.EncryptToBase64(HMACMode.SHA384, "1234", "1111")
            .ShouldBe("Bemy8D5Yqh/p7kDreTkBQ0HKl5ajCepMoz3gblxj02R1KoPOTIb1JDqlVE5/IvcY");

        HMACEncrypt.EncryptToBase64(HMACMode.SHA512, "1234", "1111")
            .ShouldBe("EFcvs1WahNpLB5A1scitbXrAXvOrgecru2r2ua2RFKsbl7T1sxIQYIu21dlgLHXgg7rCg4F6+zJUspl7psrNGQ==");
    }

    [Fact]
    public void EncryptToHex_Test()
    {
        HMACEncrypt.EncryptToHex(HMACMode.MD5, "1234", "1111")
            .ShouldBe("a96b5ba51669a2d6f32a6e4d739d5108");

        HMACEncrypt.EncryptToHex(HMACMode.SHA1, "1234", "1111")
            .ShouldBe("bd4150b4e18682b11a8b4e694cdf3a4c36d0aad9");

        HMACEncrypt.EncryptToHex(HMACMode.SHA256, "1234", "1111")
            .ShouldBe("388472000d021c1f7d089c6472750cdf80b91775dd7ff69a59d324216cdff15e");

        HMACEncrypt.EncryptToHex(HMACMode.SHA384, "1234", "1111")
            .ShouldBe("05e9b2f03e58aa1fe9ee40eb7939014341ca9796a309ea4ca33de06e5c63d364752a83ce4c86f5243aa5544e7f22f718");

        HMACEncrypt.EncryptToHex(HMACMode.SHA512, "1234", "1111")
            .ShouldBe("10572fb3559a84da4b079035b1c8ad6d7ac05ef3ab81e72bbb6af6b9ad9114ab1b97b4f5b31210608bb6d5d9602c75e083bac283817afb3254b2997ba6cacd19");
    }

    [Fact]
    public void Encrypt_Test()
    {
        HMACEncrypt.Encrypt(HMACMode.MD5, "1234", "1111")
            .ShouldBe(new byte[]
            {
                169, 107, 91, 165, 22, 105, 162, 214,
                243, 42, 110, 77, 115, 157, 81, 8
            });

        HMACEncrypt.Encrypt(HMACMode.SHA1, "1234", "1111")
            .ShouldBe(new byte[]
            {
                189, 65, 80, 180, 225, 134, 130, 177,
                26, 139, 78, 105, 76, 223, 58, 76,
                54, 208, 170, 217
            });

        HMACEncrypt.Encrypt(HMACMode.SHA256, "1234", "1111")
            .ShouldBe(new byte[]
            {
                56, 132, 114, 0, 13, 2, 28, 31,
                125, 8, 156, 100, 114, 117, 12, 223,
                128, 185, 23, 117, 221, 127, 246, 154,
                89, 211, 36, 33, 108, 223, 241, 94
            });

        HMACEncrypt.Encrypt(HMACMode.SHA384, "1234", "1111")
            .ShouldBe(new byte[]
            {
                5, 233, 178, 240, 62, 88, 170, 31,
                233, 238, 64, 235, 121, 57, 1, 67,
                65, 202, 151, 150, 163, 9, 234, 76,
                163, 61, 224, 110, 92, 99, 211, 100,
                117, 42, 131, 206, 76, 134, 245, 36,
                58, 165, 84, 78, 127, 34, 247, 24
            });

        HMACEncrypt.Encrypt(HMACMode.SHA512, "1234", "1111")
            .ShouldBe(new byte[]
            {
                16, 87, 47, 179, 85, 154, 132, 218,
                75, 7, 144, 53, 177, 200, 173, 109,
                122, 192, 94, 243, 171, 129, 231, 43,
                187, 106, 246, 185, 173, 145, 20, 171,
                27, 151, 180, 245, 179, 18, 16, 96,
                139, 182, 213, 217, 96, 44, 117, 224,
                131, 186, 194, 131, 129, 122, 251, 50,
                84, 178, 153, 123, 166, 202, 205, 25
            });
    }
}