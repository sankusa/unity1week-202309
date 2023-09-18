using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib {
    public class TextureCreator
    {
        [MenuItem("SankusaLib/TextureCreator")]
        public static void Create() {
            Texture2D texture = new Texture2D(256, 256, TextureFormat.RGBA32, false);
            PaintColorCircle(texture);
            texture.Apply();
            System.IO.File.WriteAllBytes(Application.dataPath + "/NewTexture.png", texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.Refresh();
        }

        private static void PaintColorCircle(Texture2D texture) {
            float radMin = 0;
            float radMax = 0;
            for(int i = 0; i< texture.width; i++) {
                for(int j = 0; j < texture.height; j++) {
                    Color color = Color.white;
                    float radius = texture.width / 2;
                    float x = i - radius;
                    float y = j - radius;
                    float d = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
                    if(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) < Mathf.Pow(radius, 2)) {
                        float radian = Mathf.Atan2(y, x);
                        radMin = Mathf.Min(radMin, radian);
                        radMax = Mathf.Max(radMax, radian);
                        float angle = radian * Mathf.Rad2Deg;
                        float r = 0;
                        if(-120 <= angle && angle <= -60) r = (angle + 120) / 60;
                        else if(-60 <= angle && angle <= 60) r = 1;
                        else if(60 <= angle && angle <= 120) r = (120 - angle) / 60;
                        float g = 0;
                        if(-180 <= angle && angle <= -120) g = (-angle - 120) / 60;
                        else if(0 <= angle && angle <= 60) g = angle / 60;
                        else if(60 <= angle && angle <= 180) g = 1;
                        float b = 0;
                        if(-180 <= angle && angle <= -60) b = 1;
                        else if(-60 <= angle && angle <= 0) b = -angle / 60;
                        else if(120 <= angle && angle <= 180) b = (angle - 120) / 60;

                        r += (1 - r) * (1  - (d / radius));
                        g += (1 - g) * (1  - (d / radius));
                        b += (1 - b) * (1  - (d / radius));

                        color = new Color(r, g, b, 1);
                    } else {
                        color = Color.black;
                    }
                    texture.SetPixel(i, j, color);
                }
            }
        }
    }
}