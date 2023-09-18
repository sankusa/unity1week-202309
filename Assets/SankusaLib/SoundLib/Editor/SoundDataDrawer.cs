using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib.SoundLib {
    [CustomPropertyDrawer(typeof(SoundData))]
    public class SoundDataDrawer : PropertyDrawer{
        private float lineHeight = EditorGUIUtility.singleLineHeight;
        private float verticalSpace = EditorGUIUtility.standardVerticalSpacing;
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            // メンバ取得
            var idProp = prop.FindPropertyRelative("id");
            var clipProp = prop.FindPropertyRelative("clip");
            var volumeTypeProp = prop.FindPropertyRelative("volumeType");
            var volumeProp = prop.FindPropertyRelative("volume");
            var volumeCurveProp = prop.FindPropertyRelative("volumeCurve");
            var pitchProp = prop.FindPropertyRelative("pitch");
            var startProp = prop.FindPropertyRelative("start");
            var endProp = prop.FindPropertyRelative("end");

            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40;

            List<Rect> rects = RectUtil.DivideRectVertical(rect, new List<float>(){1, 1, 1, 1}, null, 2, 2);

            // ---- 1行目 ----
            List<Rect> line1Rects = RectUtil.DivideRectHorizontal(rects[0], new List<float>(){1, 1}, null, 2, 2);
            EditorGUI.PropertyField(line1Rects[0], idProp);

            EditorGUI.PropertyField(line1Rects[1], clipProp);

            // ---- 2行目 ----
            List<Rect> line2Rects = RectUtil.DivideRectHorizontal(rects[1], new List<float>(){1, 1, 2}, null, 2, 2);
            EditorGUI.PropertyField(line2Rects[0], volumeTypeProp, new GUIContent("音量"));
            if(volumeTypeProp.enumValueIndex == (int)VolumeType.Constant) {
                volumeProp.floatValue = EditorGUI.Slider(line2Rects[1], volumeProp.floatValue, 0f, 1f);
            } else {
                EditorGUI.PropertyField(line2Rects[1], volumeCurveProp, GUIContent.none);
            }
            

            EditorGUI.PropertyField(line2Rects[2], pitchProp);

            // ---- 3行目 ----
            List<Rect> line3Rects = RectUtil.DivideRectHorizontal(rects[2], new List<float>(){88, 60, 50, 60, 20}, new List<int>(){0, 1, 2, 3, 4}, 2, 2);
            AudioClip clip = (AudioClip)clipProp.objectReferenceValue;
            float clipLen = clip != null ? clip.length : 0;
            // EditorGUI.LabelField(rects[2], "再生範囲[開始:" + (clipLen * startProp.floatValue).ToString("0.0") + " - 終了:" + (clipLen * endProp.floatValue).ToString("0.0") + "]");
            EditorGUI.LabelField(line3Rects[0], "再生範囲[開始:");
            startProp.floatValue = Mathf.Max(0, EditorGUI.FloatField(line3Rects[1], clipLen * startProp.floatValue) / clipLen);
            EditorGUI.LabelField(line3Rects[2], " - 終了:");
            endProp.floatValue = Mathf.Max(startProp.floatValue, EditorGUI.FloatField(line3Rects[3], clipLen * endProp.floatValue) / clipLen);
            EditorGUI.LabelField(line3Rects[4], "]");

            // ---- 4行目 ----
            List<Rect> line4Rects = RectUtil.DivideRectHorizontal(rects[3], new List<float>(){40, 1, 40}, new List<int>(){0, 2}, 2, 2);
            EditorGUI.LabelField(line4Rects[0], "0.0");

            float start = startProp.floatValue;
            float end = endProp.floatValue;
            EditorGUI.MinMaxSlider(line4Rects[1], ref start, ref end, 0, 1);
            startProp.floatValue = start;
            endProp.floatValue = end;

            EditorGUI.LabelField(line4Rects[2], clipLen.ToString("0.0"));

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (lineHeight + verticalSpace) * 4;
        }
    }
}