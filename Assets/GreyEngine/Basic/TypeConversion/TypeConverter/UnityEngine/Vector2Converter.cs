using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class Vector2Converter : TypeConverter
    {
        public Vector2Converter() {
            Type = typeof(Vector2);
            InitialString = ValueToString(Vector2.zero);
        }
        override public string ValueToString(object value) {
            Vector2 vector = (Vector2)value;
            return vector.x.ToString() + "," + vector.y.ToString();
        }
        override public object StringToValue(string valueString) {
            string[] values = valueString.Split(',');
            return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.Vector2Field(rect, label, (Vector2)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.Vector2Field(label, (Vector2)StringToValue(valueString)));
        }
#endif
    }
}