using System.Text.RegularExpressions;

namespace System
{
    public static class SnowDecimalExtension
    {
        /// <summary>
        /// 转中文大写
        /// </summary>
        /// <param name="number">数值</param>
        /// <returns></returns>
        public static string ToChinese(this decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }

        /// <summary>
        /// 转带单位的
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digit">小数位</param>
        /// <returns></returns>
        public static string ToUnit(this decimal number, int digit = 2)
        {
            decimal measurement = 10000;
            if (number < measurement)
            {
                return number.ToString("F" + digit);
            }
            string unit = "万";
            var tempNumber = number / measurement;
            if (tempNumber >= measurement)
            {
                tempNumber /= measurement;
                unit = "亿";
            }
            return tempNumber.ToString("F" + digit) + unit;
        }
    }
}