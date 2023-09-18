using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeedLib
{
    public static class ColorExtension
    {
        public static string ToColorCodeRGB(this Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        public static Color FromHtmlString(this Color color, string htmlString)
        {
            bool success = ColorUtility.TryParseHtmlString(htmlString, out Color newColor);
            if(!success)
            {
                Debug.LogWarning("Parse failed. htmlString = " + htmlString);
                return color;
            }

            return newColor;
        }

        public static Color OverWrite(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            return new Color(
                r ?? color.r,
                g ?? color.g,
                b ?? color.b,
                a ?? color.a
            );
        }

        public static Color OverWriteWithoutAlpha(this Color color, Color overWriteColor)
        {
            return color.OverWrite(overWriteColor.r, overWriteColor.g, overWriteColor.b);
        }
    }
}