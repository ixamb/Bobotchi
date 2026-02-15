using UnityEngine;

namespace Core.Runtime.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetR(this Color color, float value)
        {
            color.r = value;
            return color;
        }

        public static Color SetG(this Color color, float value)
        {
            color.g = value;
            return color;
        }

        public static Color SetB(this Color color, float value)
        {
            color.b = value;
            return color;
        }

        public static Color SetA(this Color color, float value)
        {
            color.a = value;
            return color;
        }

        public static Color SetRGBOnly(this Color color, float r, float g, float b)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            return color;
        }

        public static Color SetRGBOnly(this Color color, Color newColor)
        {
            color.r = newColor.r;
            color.g = newColor.g;
            color.b = newColor.b;
            return color;
        }
    }
}