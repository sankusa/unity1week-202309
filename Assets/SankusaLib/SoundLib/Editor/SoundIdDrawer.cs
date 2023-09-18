using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SankusaLib.CustomPopupLib;
using System.Linq;

namespace SankusaLib.SoundLib {
    [CustomPropertyDrawer(typeof(SoundIdAttribute))]
    public class FruitIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.String) {
                property.stringValue = CustomPopup.SoundIdPopup(rect, label.text, property.stringValue);
            } else {
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            }
        }
    }
}