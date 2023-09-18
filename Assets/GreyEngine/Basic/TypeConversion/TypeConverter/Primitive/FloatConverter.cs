using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class FloatConverter : TypeConverter
    {
        public FloatConverter() {
            Type = typeof(float);
            SimpleTypeName = "float";
            InitialString = "0";
        }
        override public string ValueToString(object value) {
            return ((float)value).ToString();
        }
        override public object StringToValue(string valueString) {
            return float.Parse(valueString);
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.FloatField(rect, label, (float)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.FloatField(label, (float)StringToValue(valueString)));
        }
#endif
    }
}