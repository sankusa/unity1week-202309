using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.LocalizeLib {
    public class CustomGUI
    {
        private static Dictionary<System.Type, ScriptableObject> scriptableObjectDictionary = new Dictionary<System.Type, ScriptableObject>();
        private static Dictionary<System.Type, int> scriptableObjectLoadWaitCounterDictionary = new Dictionary<System.Type, int>();
        private static float scriptableObjectLoadInterval = 1f;
        private const int editorUpdateCountPerSecond = 200;

        private static void Update() {
            List<System.Type> keys = scriptableObjectLoadWaitCounterDictionary.Keys.ToList();
            foreach(System.Type key  in keys) {
                scriptableObjectLoadWaitCounterDictionary[key]--;
                if(scriptableObjectLoadWaitCounterDictionary[key] == 0) {
                    scriptableObjectLoadWaitCounterDictionary.Remove(key);
                }
            }
            if(scriptableObjectLoadWaitCounterDictionary.Count == 0) EditorApplication.update -= Update;
        }

        public static void LocalizedLabelField(Rect rect, string key) {
            // ScriptableObject取得
            if(!scriptableObjectDictionary.ContainsKey(typeof(Localize))) {
                // ロード待機判定
                if(!scriptableObjectLoadWaitCounterDictionary.ContainsKey(typeof(Localize))) {
                    List<Localize> scriptableObjects = AssetUtil.LoadAllAssets<Localize>();
                    if(scriptableObjects.Count == 1) {
                        scriptableObjectDictionary.Add(typeof(Localize), scriptableObjects[0]);
                    } else {
                        // 待機用カウンターに加算
                        if(scriptableObjectLoadWaitCounterDictionary.Count == 0) {
                            EditorApplication.update += Update;
                        }
                        scriptableObjectLoadWaitCounterDictionary.Add(typeof(Localize), (int)(scriptableObjectLoadInterval * editorUpdateCountPerSecond));
                        Debug.LogWarning("There are " + scriptableObjects.Count + " " + typeof(Localize).Name);
                    }
                }
            }
            // GUI
            if(!scriptableObjectDictionary.ContainsKey(typeof(Localize))) {
                EditorGUI.DrawRect(rect, new Color(1, 0, 0, 0.2f));
                return;
            } else {
                Localize localize = (Localize)scriptableObjectDictionary[typeof(Localize)];
                EditorGUI.LabelField(rect, localize.GetTableContainer(localize.DefaultLanguage).FindString(key));
                return;
            }
        }

        public static string PopupFromScriptableObject<T>(Rect rect, string text, System.Func<T, IEnumerable<string>> textsCreator) where T : ScriptableObject {
            // ScriptableObject取得
            if(!scriptableObjectDictionary.ContainsKey(typeof(T))) {
                // ロード待機判定
                if(!scriptableObjectLoadWaitCounterDictionary.ContainsKey(typeof(T))) {
                    List<T> scriptableObjects = AssetUtil.LoadAllAssets<T>();
                    if(scriptableObjects.Count == 1) {
                        scriptableObjectDictionary.Add(typeof(T), scriptableObjects[0]);
                    } else {
                        // 待機用カウンターに加算
                        if(scriptableObjectLoadWaitCounterDictionary.Count == 0) {
                            EditorApplication.update += Update;
                        }
                        scriptableObjectLoadWaitCounterDictionary.Add(typeof(T), (int)(scriptableObjectLoadInterval * editorUpdateCountPerSecond));
                        Debug.LogWarning("There are " + scriptableObjects.Count + " " + typeof(T).Name);
                    }
                }
            }
            // GUI
            if(!scriptableObjectDictionary.ContainsKey(typeof(T))) {
                string ret = EditorGUI.TextField(rect, text);
                EditorGUI.DrawRect(rect, new Color(1, 0, 0, 0.2f));
                return ret;
            } else {
                string ret = Popup(rect, text, textsCreator((T)scriptableObjectDictionary[typeof(T)]));
                return ret;
            }
        }

        public static string Popup(Rect rect, string text, IEnumerable<string> options) {
            List<string> displayedOptions = new List<string>(){""};
            displayedOptions.AddRange(options);
            int index = displayedOptions.IndexOf(text);
            if(index == -1) index = 0;
            index = EditorGUI.Popup(rect, index, displayedOptions.ToArray());
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