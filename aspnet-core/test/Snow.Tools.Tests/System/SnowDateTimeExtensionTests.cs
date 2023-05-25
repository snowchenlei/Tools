using Shouldly;
using Xunit;

namespace Snow.Tools.Tests.System;

public class SnowDateTimeExtensionTests
{
    [Fact]
    public void Should_ToUnixTimeMilliseconds()
    {
        var milliseconds = DateTime.Now.ToUnixTimeMilliseconds();
        milliseconds.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void Should_ToUnixTimeSeconds()
    {
        var seconds = DateTime.Now.ToUnixTimeSeconds();
        seconds.ShouldBeGreaterThan(0);
    }
}