using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SankusaLib {
    public class AssetUtil
    {
        // ================ プレハブ ================
        // プレハブ検索＋インスタンス化
        public static void InstantiatePrefab(string keyword) {
            List<GameObject> prefabs = FindAssets<GameObject>(keyword);
            if(prefabs.Count == 0) {
                Debug.LogError("キーワードに該当するプレハブが存在しません。Keyword = " + keyword);
            } else {
                if(prefabs.Count > 1) Debug.LogWarning("キーワードに該当するプレハブが複数存在したため、内１つをインスタンス化しました。 Keyword = " + keyword);
                PrefabUtility.InstantiatePrefab(prefabs[0]);
            }
        }

        // ================ ScriptableObject ================
        // ScriptableObjectをロード、無ければ作る作る
        public static T SafeLoadScriptableObject<T>(string path, HideFlags hideFlag) where T : ScriptableObject {
            T obj = AssetDatabase.LoadAssetAtPath<T>(path);
            if(obj == null) {
                obj = ScriptableObject.CreateInstance<T>();
                FolderUtil.SafeCreateDirectory(Path.GetDirectoryName(path));
                AssetDatabase.CreateAsset(obj, path);
                obj.hideFlags = hideFlag;
                EditorUtility.SetDirty(obj);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            return obj;
        }
        public static T SafeLoadScriptableObject<T>(string path) where T : ScriptableObject {
            return SafeLoadScriptableObject<T>(path, HideFlags.None);
        }

        // ================ アセット全般 ================
        // アセット検索
        public static List<T> FindAssets<T>(string keyword) where T : Object {
            List<T> prefabs = new List<T>();
            List<string> paths = GetAssetPaths<T>();
            for(int i = paths.Count - 1; i >= 0; i--) {
                if(paths[i].IndexOf(keyword) == -1) {
                    paths.RemoveAt(i);
                }
            }
            foreach(string path in paths) {
                prefabs.Add((T)AssetDatabase.LoadAssetAtPath<T>(path));
            }
            return prefabs;
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

        // プロジェクト内の対象型のパスを全件取得
        public static List<string> GetAssetPaths<T>() where T : Object {
            List<string> list = new List<string>(); 
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            return list;
        }

    }
}