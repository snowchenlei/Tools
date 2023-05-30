using Shouldly;
using Xunit;

namespace Snow.Tools.Tests.System;

public class SnowDateTimeExtension_Tests
{
    [Fact]
    public void ToUnixTimeMilliseconds_Test()
    {
        new DateTime(2023, 1, 1).ToUnixTimeMilliseconds().ShouldBe(1672502400000);
    }

    [Fact]
    public void ToUnixTimeSeconds_Test()
    {
        new DateTime(2023, 1, 1).ToUnixTimeSeconds().ShouldBe(1672502400);
    }
}