using System.Text;
using Shouldly;
using Xunit;
using Convert = System.Convert;

namespace Snow.Tools.Tests.System;

public class SnowByteExtension_Tests
{
    [Fact]
    public void ToMd5_Test()
    {
        var m0 = new byte[] { 0, 0, 0, 0 }.ToMd5();
        Convert.ToHexString(m0).ShouldBe("F1D3FF8443297732862DF21DC4E57262");

        var m1 = new byte[] { 1, 1, 1, 1 }.ToMd5();
        Convert.ToHexString(m1).ShouldBe("3B5B9852567EF7618AAC7F5F2D74EF74");
    }

    [Fact]
    public async Task ToMd5_Throw_ArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(Array.Empty<byte>().ToMd5()));
    }
}