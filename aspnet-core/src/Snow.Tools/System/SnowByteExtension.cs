using System.Text;

namespace System
{
    public static class SnowByteExtension
    {
        /// <summary>
        /// 将要读取文件头信息的文件的byte数组转换成string类型表示
        /// 下面这段代码就是用来对文件类型作验证的方法，将字节数组的前四位转换成16进制字符串，并且转换的时候，要先和0xFF做一次与运算。
        /// 这是因为，整个文件流的字节数组中，有很多是负数，进行了与运算后，可以将前面的符号位都去掉，
        /// 这样转换成的16进制字符串最多保留两位，如果是正数又小于10，那么转换后只有一位，
        /// 需要在前面补0，这样做的目的是方便比较，取完前四位这个循环就可以终止了
        /// </summary>
        /// <param name="src">要读取文件头信息的文件的byte数组</param>
        /// <returns>文件头信息</returns>
        public static string ToHexString(this byte[] src)
        {
            StringBuilder builder = new StringBuilder();
            if (src == null || src.Length <= 0)
            {
                return null;
            }
            string hv;
            foreach (var b in src)
            {
                // 以十六进制（基数 16）无符号整数形式返回一个整数参数的字符串表示形式，并转换为大写
                hv = Convert.ToString(b & 0xFF, 16).ToUpper();
                if (hv.Length < 2)
                {
                    builder.Append(0);
                }
                builder.Append(hv);
            }
            return builder.ToString();
        }
    }
}