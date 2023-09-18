using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib.LocalizeLib {
    [CustomPropertyDrawer(typeof(StringData))]
    public class DataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            List<Rect> rects = RectUtil.DivideRect(position, new List<float>{1,1}, null, 2, 2);

            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 40;

            EditorGUI.PropertyField(rects[0], property.FindPropertyRelative("key"));
            EditorGUI.PropertyField(rects[1], property.FindPropertyRelative("value"));

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }
    }
}