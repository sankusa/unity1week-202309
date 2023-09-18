using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic {
    public class ColorConverter : TypeConverter
    {
        public ColorConverter() {
            Type = typeof(Color);
            InitialString = ValueToString(Color.white);
        }
        override public string ValueToString(object value) {
            Color color = (Color)value;
            return color.r.ToString() + "," + color.g.ToString() + "," + color.b.ToString() + "," + color.a.ToString();
        }
        override public object StringToValue(string valueString) {
            string[] values = valueString.Split(',');
            return new Color(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.ColorField(rect, label, (Color)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.ColorField(label, (Color)StringToValue(valueString)));
        }
#endif
    }
}