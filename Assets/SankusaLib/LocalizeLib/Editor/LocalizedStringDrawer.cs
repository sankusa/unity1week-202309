using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.LocalizeLib {
    [CustomPropertyDrawer(typeof(LocalizedStringAttribute))]
    public class LocalizeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            List<Rect> rects = RectUtil.DivideRect(rect, new List<float>(){1, 1}, null, 2, 2);

            if(property.propertyType == SerializedPropertyType.String) {
                property.stringValue = CustomGUI.PopupFromScriptableObject<Localize>(rects[0], property.stringValue, x => x.GetAllStringKeys(x.DefaultLanguage));
            } else {
                EditorGUI.PropertyField(rects[0], property, GUIContent.none);
            }
            CustomGUI.LocalizedLabelField(rects[1], property.stringValue);
        }
    }
}