using System.Security.Cryptography;
using System.Text;
using Snow.Tools;
using Snow.Tools.Security.Cryptography;

namespace System
{
    public static class SnowByteExtension
    {
        public static byte[] ToMd5(this byte[] source)
        {
            Check.NotNullOrEmpty(source, nameof(source));

            return MD5Encrypt.Encrypt(source);
        }
    }
}