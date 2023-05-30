using System.Security.Cryptography;
using System.Text;
using Snow.Tools;

namespace System
{
    public static class SnowByteExtension
    {
        public static byte[]? ToMd5(this byte[] source)
        {
            Check.NotNullOrEmpty(source, nameof(source));

            using var md5 = MD5.Create();
            md5.ComputeHash(source);
            return md5.Hash;
        }
    }
}