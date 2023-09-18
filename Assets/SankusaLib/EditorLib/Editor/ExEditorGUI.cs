using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib.EditorLib {
    public static class ExEditorGUI {
        public static float horizontalSpace = 4;
        public static float verticalSpace = 4;

        public static void ChildsPropertyField(Rect rect, SerializedProperty property, Orientation orientation) {
            var childList = new List<SerializedProperty>();
            var weightList = new List<float>();
            var rects = new List<Rect>();
            var endProp = property.GetEndProperty();

            while(property.NextVisible(true)) {
                if(SerializedProperty.EqualContents(property, endProp)) break;
                
                childList.Add(property.Copy());
                weightList.Add(1);
            }

            if(orientation == Orientation.Horizontal) {
                rects = RectUtil.DivideRectHorizontal(rect, weightList, null, horizontalSpace);
            } else {
                rects = RectUtil.DivideRectVertical(rect, weightList, null, verticalSpace);
            }
            
            for(int i = 0; i < childList.Count; i++) {
                EditorGUI.PropertyField(rects[i], childList[i], GUIContent.none);
            }
        }
    }
}