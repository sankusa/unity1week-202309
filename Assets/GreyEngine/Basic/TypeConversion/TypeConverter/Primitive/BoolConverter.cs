using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class BoolConverter : TypeConverter
    {
        public BoolConverter() {
            Type = typeof(bool);
            SimpleTypeName = "bool";
            InitialString = "false";
        }
        override public string ValueToString(object value) {
            return ((bool)value).ToString();
        }
        override public object StringToValue(string valueString) {
            return bool.Parse(valueString);
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.Toggle(rect, label, (bool)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.Toggle(label, (bool)StringToValue(valueString)));
        }
#endif
    }
}