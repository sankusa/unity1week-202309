using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib.LocalizeLib {
    [CustomPropertyDrawer(typeof(StringTable))]
    public class TableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, GUIContent.none);
        }
    }
}