using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class IntConverter : TypeConverter
    {
        public IntConverter() {
            Type = typeof(int);
            SimpleTypeName = "int";
            InitialString = "0";
        }
        override public string ValueToString(object value) {
            return ((int)value).ToString();
        }
        override public object StringToValue(string valueString) {
            return int.Parse(valueString);
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.IntField(rect, label, (int)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.IntField(label, (int)StringToValue(valueString)));
        }
#endif
    }
}