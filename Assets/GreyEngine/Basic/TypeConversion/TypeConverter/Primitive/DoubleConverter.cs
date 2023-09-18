using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class DoubleConverter : TypeConverter
    {
        public DoubleConverter() {
            Type = typeof(double);
            SimpleTypeName = "double";
            InitialString = "0";
        }
        override public string ValueToString(object value) {
            return ((double)value).ToString();
        }
        override public object StringToValue(string valueString) {
            return double.Parse(valueString);
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.DoubleField(rect, label, (double)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.DoubleField(label, (double)StringToValue(valueString)));
        }
#endif
    }
}