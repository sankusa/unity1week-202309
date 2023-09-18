using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class Vector3Converter : TypeConverter
    {
        public Vector3Converter() {
            Type = typeof(Vector3);
            InitialString = ValueToString(Vector3.zero);
        }
        override public string ValueToString(object value) {
            Vector3 vector = (Vector3)value;
            return vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString();
        }
        override public object StringToValue(string valueString) {
            string[] values = valueString.Split(',');
            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.Vector3Field(rect, label, (Vector3)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.Vector3Field(label, (Vector3)StringToValue(valueString)));
        }
#endif
    }
}