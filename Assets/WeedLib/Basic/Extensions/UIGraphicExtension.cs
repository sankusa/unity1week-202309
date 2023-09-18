using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeedLib
{
    public static class UIGraphicExtension
    {
        public static void OverWriteColor(this Graphic graphic, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            graphic.color = new Color(
                r ?? graphic.color.r,
                g ?? graphic.color.g,
                b ?? graphic.color.b,
                a ?? graphic.color.a
            );
        }

        public static void OverWriteAlpha(this Graphic graphic, float alpha)
        {
            graphic.OverWriteColor(a: alpha);
        }

        public static void OverWriteColorWithoutAlpha(this Graphic graphic, Color color)
        {
            graphic.OverWriteColor(color.r, color.g, color.b);
        }

        public static void OverWriteColorAndAlpha(this Graphic graphic, Color color, float alpha)
        {
            graphic.OverWriteColor(color.r, color.g, color.b, alpha);
        }
    }
}