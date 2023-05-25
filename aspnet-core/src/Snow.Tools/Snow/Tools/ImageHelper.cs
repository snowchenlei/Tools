using SixLabors.ImageSharp.Formats.Jpeg;

namespace Snow.Tools;

/// <summary>
/// 图片操作
/// </summary>
public static class ImageHelper
{
    /// <summary>
    /// 二分法压缩
    /// </summary>
    /// <param name="fileBytes">图片字节</param>
    /// <param name="targetSize">目标字节</param>
    /// <returns>压缩后的图片</returns>
    public static async Task<byte[]> DichotomyCompressAsync(byte[] fileBytes, long targetSize)
    {
        if (fileBytes.Length <= targetSize)
        {
            return fileBytes;
        }

        using Image image = Image.Load(fileBytes);
        long minQuality = 1;
        long maxQuality = 100;
        int iterations = 10; // 控制二分搜索的最大次数，避免死循环
        for (int i = 0; i < iterations; i++)
        {
            long midQuality = (minQuality + maxQuality) / 2;
            using var outputImage = new MemoryStream();
            await image.SaveAsync(outputImage, new JpegEncoder() { Quality = (int)midQuality });
            var newSize = outputImage.Length;
            if (newSize <= targetSize)
            {
                return outputImage.ToArray();
            }

            maxQuality = midQuality - 1;
        }

        return fileBytes;
    }
}