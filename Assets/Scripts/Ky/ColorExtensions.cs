using UnityEngine;

namespace Ky
{
    public static class ColorExtensions
    {
        public static string EncapsulateString(this Color color, string text)
        {
            return $"<color=#{color.ToHex()}>{text}</color>";
        }

        public static string ToHex(this Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }

        public static Color SetR(this Color c, float value)
        {
            c[0] = value;
            return c;
        }

        public static Color SetG(this Color c, float value)
        {
            c[1] = value;
            return c;
        }

        public static Color SetB(this Color c, float value)
        {
            c[2] = value;
            return c;
        }

        public static Color SetA(this Color c, float value)
        {
            c[3] = value;
            return c;
        }
    }
}