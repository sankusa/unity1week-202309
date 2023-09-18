using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.CustomPopupLib {
    [CustomPropertyDrawer(typeof(AssetPathPopupAttribute))]
    public class AssetPathPopupDrawer : PropertyDrawer
    {
        private static Dictionary<System.Type, object> pathsUnderResourcesDictionary = new Dictionary<System.Type, object>();
        private static Dictionary<System.Type, object> pathsDictionary = new Dictionary<System.Type, object>();

        private const string RESOURCES_FOLDER_NAME = "Resources";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var assetPath = (AssetPathPopupAttribute)attribute;
            if(property.propertyType == SerializedPropertyType.String && assetPath.type.IsSubclassOf(typeof(Object))) {
                if(assetPath.underResources) {
                    List<string> paths;
                    if(pathsUnderResourcesDictionary.ContainsKey(assetPath.type)) {
                        paths = (List<string>)pathsUnderResourcesDictionary[assetPath.type];
                    } else {
                        paths = GetAssetPathsUnderResources(assetPath.type);
                        pathsUnderResourcesDictionary.Add(assetPath.type, paths);
                        EditorApplication.projectChanged -= UpdatePaths;
                        EditorApplication.projectChanged += UpdatePaths;
                    }
                    property.stringValue = CustomPopup.Popup(position, property.stringValue, paths, label);
                } else {
                    List<string> paths;
                    if(pathsDictionary.ContainsKey(assetPath.type)) {
                        paths = (List<string>)pathsDictionary[assetPath.type];
                    } else {
                        paths = GetAssetPaths(assetPath.type);
                        pathsDictionary.Add(assetPath.type, paths);
                        EditorApplication.projectChanged -= UpdatePaths;
                        EditorApplication.projectChanged += UpdatePaths;
                    }
                    property.stringValue = CustomPopup.Popup(position, property.stringValue, paths, label);
                }
            } else {
                EditorGUI.PropertyField(position, property);
            }
        }

        private void UpdatePaths() {
            for(int i = 0; i < pathsUnderResourcesDictionary.Count; i++) {
                System.Type type = pathsUnderResourcesDictionary.ElementAt(i).Key;
                pathsUnderResourcesDictionary[type] = GetAssetPathsUnderResources(type);
            }
            for(int i = 0; i < pathsDictionary.Count; i++) {
                System.Type type = pathsDictionary.ElementAt(i).Key;
                pathsDictionary[type] = GetAssetPaths(type);
            }
        }

        // プロジェクト内の対象型のパスを全件取得
        public static List<string> GetAssetPaths(System.Type type) {
            List<string> list = new List<string>(); 
            
            string[] guids = AssetDatabase.FindAssets("t:" + type.Name);

            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }
            return list;
        }

        public static List<string> GetAssetPathsUnderResources(System.Type type) {
            List<string> list = new List<string>(); 
            
            string[] guids = AssetDatabase.FindAssets("t:" + type.Name);

            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                int resourcesIndex = path.IndexOf(RESOURCES_FOLDER_NAME);
                
                
                if(resourcesIndex != -1) {
                    int pathStartIndex = resourcesIndex + RESOURCES_FOLDER_NAME.Length + 1;
                    int extensionIndex = path.LastIndexOf('.');
                    string pathUnderResources = path.Substring(pathStartIndex, extensionIndex - pathStartIndex);
                    list.Add(pathUnderResources);
                }
            }
            return list;
        }
    }
}