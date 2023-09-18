using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreyEngine.Basic.Utils {
    public class TextureUtil
    {
        // テクスチャ生成
        public static Texture2D CreateOneDotTexture(Color color) {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0,0, color);
            texture.Apply();
            return texture;
        }
    }
}