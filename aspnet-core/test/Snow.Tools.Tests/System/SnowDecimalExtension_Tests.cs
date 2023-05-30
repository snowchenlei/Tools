using Shouldly;
using Xunit;

namespace Snow.Tools.Tests.System;

public class SnowDecimalExtension_Tests
{
    [Fact]
    public void ToChinese_Test()
    {
        10M.ToChinese().ShouldBe("壹拾元整");
        0M.ToChinese().ShouldBe("零元整");
    }

    [Fact]
    public void ToUnit_Test()
    {
        0M.ToUnit().ShouldBe("0.00");
        154000M.ToUnit().ShouldBe("15.40万");
        154030M.ToUnit(3).ShouldBe("15.403万");
    }
}