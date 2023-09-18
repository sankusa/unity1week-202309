using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic.TypeConversion {
    public class BoundsConverter : TypeConverter
    {
        public BoundsConverter() {
            Type = typeof(Bounds);
            InitialString = "0,0,0,0,0,0";
        }
        override public string ValueToString(object value) {
            Bounds bounds = (Bounds)value;
            return bounds.center.x + "," + bounds.center.y + "," + bounds.center.z + "," +
                   bounds.size.x + "," + bounds.size.y + "," + bounds.size.z;
        }
        override public object StringToValue(string valueString) {
            string[] values = valueString.Split(',');
            return new Bounds(new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])),
                              new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5])));
        }
#if UNITY_EDITOR
        override public string Field(Rect rect, string label, string valueString) {
            return ValueToString(EditorGUI.BoundsField(rect, label, (Bounds)StringToValue(valueString)));
        }
        override public string FieldLayout(string label, string valueString) {
            return ValueToString(EditorGUILayout.BoundsField(label, (Bounds)StringToValue(valueString)));
        }
#endif
    }
}