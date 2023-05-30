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
        m0.ShouldNotBeNull();
        m0.ShouldBe(new byte[]
        {
            241, 211, 255, 132,
            67, 41, 119, 50,
            134, 45, 242, 29,
            196, 229, 114, 98
        });
        var m1 = new byte[] { 1, 1, 1, 1 }.ToMd5();
        m1.ShouldNotBeNull();
        m1.ShouldBe(new byte[]
        {
            59, 91, 152, 82,
            86, 126, 247, 97,
            138, 172, 127, 95,
            45, 116, 239, 116
        });
    }

    [Fact]
    public async Task ToMd5_Throw_ArgumentNullException()
    {
        await Should.ThrowAsync<ArgumentException>(
            async () => await Task.FromResult(Array.Empty<byte>().ToMd5()));
    }
}