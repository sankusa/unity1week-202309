using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.EditorLib
{
    public class DragAndDropUtil
    {
        public static T GetObject<T>(Rect dropRect) where T : Object
        {
            return GetObjects(dropRect)?.OfType<T>().FirstOrDefault();
        }
        
        public static List<T> GetObjects<T>(Rect dropRect) where T : Object
        {
            return GetObjects(dropRect)?.OfType<T>().ToList();
        }

        private static Object[] GetObjects(Rect dropRect)
        {
            // カーソルが範囲外ならスルー
            if(!dropRect.Contains(Event.current.mousePosition)) return null;

            // カーソルの見た目をドラッグ用に変更
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

            // ドロップでなければ終了
            if(Event.current.type != EventType.DragPerform) return null;

            // ドロップを受け入れる
            DragAndDrop.AcceptDrag();

            // イベントを使用済みに
            Event.current.Use();

            return DragAndDrop.objectReferences;
        }
    }
}