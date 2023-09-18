using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SankusaLib.EditorLib
{
    public class EditorGUISplitView
    {

        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        Orientation splitOrientation;
        float splitNormalizedPosition;
        bool resize;
        public Vector2 scrollPosition;
        Rect availableRect;


        public EditorGUISplitView(Orientation splitOrientation) {
            splitNormalizedPosition = 0.5f;
            this.splitOrientation = splitOrientation;
        }

        public void BeginSplitView() {
            Rect tempRect;

            if(splitOrientation == Orientation.Horizontal)
                tempRect = EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth(true));
            else 
                tempRect = EditorGUILayout.BeginVertical (GUILayout.ExpandHeight(true));
            
            if (tempRect.width > 0.0f) {
                availableRect = tempRect;
            }
            if(splitOrientation == Orientation.Horizontal)
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(availableRect.width * splitNormalizedPosition));
            else
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(availableRect.height * splitNormalizedPosition));
        }

        public void Split() {
            GUILayout.EndScrollView();
            ResizeSplitFirstView ();
        }

        public void EndSplitView() {

            if(splitOrientation == Orientation.Horizontal)
                EditorGUILayout.EndHorizontal ();
            else 
                EditorGUILayout.EndVertical ();
        }

        private void ResizeSplitFirstView(){

            Rect resizeHandleRect;

            if(splitOrientation == Orientation.Horizontal)
                resizeHandleRect = new Rect (availableRect.width * splitNormalizedPosition, availableRect.y, 2f, availableRect.height);
            else
                resizeHandleRect = new Rect (availableRect.x,availableRect.height * splitNormalizedPosition, availableRect.width, 2f);

            GUI.DrawTexture(resizeHandleRect,EditorGUIUtility.whiteTexture);

            if(splitOrientation == Orientation.Horizontal)
                EditorGUIUtility.AddCursorRect(resizeHandleRect,MouseCursor.ResizeHorizontal);
            else
                EditorGUIUtility.AddCursorRect(resizeHandleRect,MouseCursor.ResizeVertical);

            if( Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition)){
                resize = true;
            }
            if(resize){
                if(splitOrientation == Orientation.Horizontal)
                    splitNormalizedPosition = Event.current.mousePosition.x / availableRect.width;
                else
                    splitNormalizedPosition = Event.current.mousePosition.y / availableRect.height;
            }
            if(Event.current.type == EventType.MouseUp)
                resize = false;        
        }
    }
}