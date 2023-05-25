namespace System;

public static class SnowGuidExtension
{
    public static int[] ToInt32Array(this Guid value)
    {
        byte[] b = value.ToByteArray();
        return new[]
        {
            BitConverter.ToInt32(b, 0),
            BitConverter.ToInt32(b, 4),
            BitConverter.ToInt32(b, 8),
            BitConverter.ToInt32(b, 12)
        };
    }
}