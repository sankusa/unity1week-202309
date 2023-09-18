using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GreyEngine.Basic.Utils {
    public class AssetUtil : MonoBehaviour
    {
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

        // プレハブ検索
        public static List<GameObject> FindPrefabs(string keyword) {
            List<GameObject> prefabs = new List<GameObject>();
            List<string> paths = GetAssetPaths<GameObject>();
            for(int i = paths.Count - 1; i >= 0; i--) {
                if(paths[i].IndexOf(keyword) == -1) {
                    paths.RemoveAt(i);
                }
            }
            foreach(string path in paths) {
                prefabs.Add((GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(path));
            }
            return prefabs;
        }

        // プレハブ検索＋インスタンス化
        public static void InstantiatePrefab(string keyword) {
            List<GameObject> prefabs = FindPrefabs(keyword);
            if(prefabs.Count == 0) {
                Debug.LogError("Prefab Not Found. Keyword = " + keyword);
            } else {
                if(prefabs.Count > 1) Debug.LogWarning("キーワードに該当するプレハブが複数存在したため、内１つをインスタンス化しました。 Keyword = " + keyword);
                PrefabUtility.InstantiatePrefab(prefabs[0]);
            }
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
        public static List<string> GetAssetPaths<T> () where T : Object {
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