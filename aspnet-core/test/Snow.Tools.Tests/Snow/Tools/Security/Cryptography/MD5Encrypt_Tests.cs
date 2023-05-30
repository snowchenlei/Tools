using System.Text;
using Shouldly;
using Snow.Tools.Security.Cryptography;
using Xunit;

namespace Snow.Tools.Tests.Snow.Tools.Security.Cryptography;

public class MD5Encrypt_Tests
{
    [Fact]
    public void Encrypt_Test()
    {
        var hashBytes = MD5Encrypt.Encrypt("123");
        hashBytes.ShouldNotBeNull();
        BitConverter.ToString(hashBytes)
            .Replace("-", "").ToUpper().ShouldBe("202CB962AC59075B964B07152D234B70");
    }

    [Fact]
    public async Task Encrypt_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.Encrypt(String.Empty)));
    }

    [Fact]
    public void Encrypt_With_Byte_Test()
    {
        var hashBytes = MD5Encrypt.Encrypt(Encoding.UTF8.GetBytes("123"));
        hashBytes.ShouldNotBeNull();
        BitConverter.ToString(hashBytes)
            .Replace("-", "").ToUpper().ShouldBe("202CB962AC59075B964B07152D234B70");
    }

    [Fact]
    public async Task Encrypt_With_Byte_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.Encrypt(Encoding.UTF8.GetBytes(String.Empty))));
    }

    [Fact]
    public void Encrypt_With_Byte_And_Encoding_Test()
    {
        var hashBytes = MD5Encrypt.Encrypt("123", Encoding.UTF8);
        hashBytes.ShouldNotBeNull();
        BitConverter.ToString(hashBytes)
            .Replace("-", "").ToUpper().ShouldBe("202CB962AC59075B964B07152D234B70");
    }

    [Fact]
    public async Task Encrypt_With_Byte_And_Encoding_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.Encrypt(String.Empty, Encoding.UTF8)));
    }

    [Fact]
    public void EncryptTo16_Test()
    {
        var result = MD5Encrypt.EncryptTo16("123");
        result.ShouldNotBeNull();
        result.ToUpper().ShouldBe("AC59075B964B0715");
    }

    [Fact]
    public async Task EncryptTo16_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.EncryptTo16(String.Empty)));
    }

    [Fact]
    public void EncryptTo32_Test()
    {
        var result = MD5Encrypt.EncryptTo32("123");
        result.ShouldNotBeNull();
        result.ToUpper().ShouldBe("202CB962AC59075B964B07152D234B70");
    }

    [Fact]
    public async Task EncryptTo32_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.EncryptTo32(String.Empty)));
    }

    [Fact]
    public void EncryptToBase64_Test()
    {
        var result = MD5Encrypt.EncryptToBase64("123");
        result.ShouldNotBeNull();
        result.ShouldBe("ICy5YqxZB1uWSwcVLSNLcA==");
    }

    [Fact]
    public async Task EncryptToBase64_Throw_ArgumentException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(MD5Encrypt.EncryptToBase64(String.Empty)));
    }
}