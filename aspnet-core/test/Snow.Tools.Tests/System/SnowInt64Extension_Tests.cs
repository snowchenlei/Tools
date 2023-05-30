using Shouldly;

namespace Snow.Tools.Tests.System;

public class SnowInt64Extension_Tests
{
    public void ToLocalDateTimeFromUnixTimeMilliseconds_Test()
    {
        1672502400000.ToLocalDateTimeFromUnixTimeMilliseconds().ShouldBe(new DateTime(2023, 1, 1));
    }

    public void ToLocalDateTimeFromUnixTimeSeconds_Test()
    {
        1672502400L.ToLocalDateTimeFromUnixTimeSeconds().ShouldBe(new DateTime(2023, 1, 1));
    }
}