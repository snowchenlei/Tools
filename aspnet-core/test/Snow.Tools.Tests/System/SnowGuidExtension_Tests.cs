using Shouldly;
using Xunit;

namespace Snow.Tools.Tests.System;

public class SnowGuidExtension_Tests
{
    [Fact]
    public void ToInt32Array_Test()
    {
        new Guid("220a35c5-b851-41b1-b73b-9d4d35845f92").ToInt32Array().ShouldBe(new[]
        {
            571094469,
            1102166097,
            1302150071,
            -1839233995
        });
    }
}