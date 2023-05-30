using Shouldly;
using Xunit;

namespace Snow.Tools.Tests.Snow.Tools;

public class ImageHelper_Tests
{
    [Fact]
    public async Task DichotomyCompress_Test()
    {
        var filePath = "test.jpg";
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileBytes = new byte[fileStream.Length];
        _ = await fileStream.ReadAsync(fileBytes, 0, fileBytes.Length);
        var maxSize = fileBytes.Length / 4 - 1;

        var result = await ImageHelper.DichotomyCompressAsync(fileBytes, maxSize);

        result.Length.ShouldBeLessThanOrEqualTo(maxSize);
    }
}