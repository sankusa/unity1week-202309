using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.SoundLib {
    public class CustomPopup
    {
        private static List<SoundDataContainer> containers = new List<SoundDataContainer>();
        private static int loadWaitCounter = 0;
        private static float scriptableObjectLoadInterval = 1f;
        private const int editorUpdateCountPerSecond = 200;

        private static void Update() {
            loadWaitCounter--;
            if(loadWaitCounter == 0) EditorApplication.update -= Update;
        }

        public static string SoundIdPopup(Rect rect, string label, string text) {
            // nullチェック
            for(int i = containers.Count - 1; i >= 0; i--) {
                if(containers[i] == null) containers.RemoveAt(i);
            }
            // ロード待機判定
            if(loadWaitCounter == 0) {
                List<SoundDataContainer> scriptableObjects = AssetUtil.LoadAllAssets<SoundDataContainer>();
                if(scriptableObjects.Count >= 1) {
                    foreach(SoundDataContainer container in scriptableObjects) {
                        if(!containers.Contains(container)) containers.Add(container);
                    }
                } else {
                    Debug.LogWarning("There are " + scriptableObjects.Count + " " + typeof(SoundDataContainer).Name);
                }
                // 待機用カウンターに加算
                loadWaitCounter = (int)(scriptableObjectLoadInterval * editorUpdateCountPerSecond);
                EditorApplication.update += Update;
            }
            // GUI
            if(containers.Count == 0) {
                string ret = EditorGUI.TextField(rect, text);
                EditorGUI.DrawRect(rect, new Color(1, 0, 0, 0.2f));
                return ret;
            } else {
                string ret = Popup(rect, label, text, containers.SelectMany(x => x.SoundDataList.Select(y => y.Id)));
                return ret;
            }
        }

        public static string Popup(Rect rect, string label, string text, IEnumerable<string> options) {
            List<string> displayedOptions = new List<string>(){""};
            displayedOptions.AddRange(options);
            int index = displayedOptions.IndexOf(text);
            if(index == -1) index = 0;
            index = EditorGUI.Popup(rect, label, index, displayedOptions.ToArray());
            string ret = displayedOptions[index];
            if(ret == "") {
                EditorGUI.DrawRect(rect, new Color(1, 0, 0, 0.2f));
            }
            return ret;
        }

        // // プロジェクト内の対象型のアセットを全てロード
        public static List<T> LoadAllAssets<T> () where T : Object {
            List<T> list = new List<T>(); 
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                list.Add(asset);
            }
            return list;
        }
    }
}