using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class StringConverter : TypeConverter
    {
        public StringConverter() {
            Type = typeof(string);
            SimpleTypeName = "string";
            InitialString = "";
        }
        override public string ValueToString(object value) {
            return (string)value;
        }
        override public object StringToValue(string valueString) {
            return valueString;
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            string ret;
            Rect labelRect = new Rect(rect.x, rect.y, rect.width / 2, rect.height);
            Rect textRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height);
            
            using(new EditorGUILayout.HorizontalScope()) {
                EditorGUI.LabelField(labelRect, label);
                ret = EditorGUI.TextArea(textRect, valueString);
            }
            return ret;
        }
        override public string FieldLayout(string label, string valueString) {
            string ret;
            using(new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));
                ret = EditorGUILayout.TextArea(valueString);
            }
            return ret;
        }
#endif
    }
}