using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GreyEngine.Basic.Utils {
    public class EditorUtil
    {
        public static void LayoutLine(float height, Color color) {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            Texture2D texture = TextureUtil.CreateOneDotTexture(color);
            boxStyle.normal.background = texture;
            GUILayout.Box(texture, boxStyle, GUILayout.Height(height), GUILayout.ExpandWidth(true));
        }
        public static void Box(Rect rect, Color color) {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            Texture2D texture = TextureUtil.CreateOneDotTexture(color);
            boxStyle.normal.background = texture;
            GUI.Box(rect, "", boxStyle);
        }
        public static void Box(Rect rect, string text, Color color) {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            Texture2D texture = TextureUtil.CreateOneDotTexture(color);
            boxStyle.normal.background = texture;
            GUI.Box(rect, text, boxStyle);
        }
        public static void LayoutBox(string text, Color color, params GUILayoutOption[] options) {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            Texture2D texture = TextureUtil.CreateOneDotTexture(color);
            boxStyle.normal.background = texture;
            GUILayout.Box(text, boxStyle, options);
        }
        // 色付きスタイル
        public static GUIStyle CreateColoredStyle(GUIStyle source, Color color) {
            GUIStyle style = new GUIStyle(source);
            style.normal.background = TextureUtil.CreateOneDotTexture(color);
            return style;
        }
        // リッチテキストラベル
        public static GUIStyle CreateRichTextStyle(GUIStyle source) {
            GUIStyle style = new GUIStyle(source);
            style.richText = true;
            return style;
        }

    }
}