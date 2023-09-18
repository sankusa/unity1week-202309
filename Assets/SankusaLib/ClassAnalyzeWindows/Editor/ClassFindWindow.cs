using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SankusaLib {
    public class ClassFindWindow : EditorWindow
    {
        string className = "";
        List<string> assemblyNames;
        List<string> typeNames;

        [MenuItem("SankusaLib/クラス検索")]
        private static void ShowWindow() {
            GetWindow<ClassFindWindow>("クラス検索");
        }

        void OnEnable() {
            assemblyNames = new List<string>();
            typeNames = new List<string>();
        }

        void OnGUI() {
            className = EditorGUILayout.TextField("クラス名", className);
            if(GUILayout.Button("検索")) {
                Find();
            } else {
                for(int i = 0; i < assemblyNames.Count; i++) {
                    using(new EditorGUILayout.HorizontalScope()) {
                        EditorGUILayout.LabelField(assemblyNames[i]);
                        EditorGUILayout.LabelField(typeNames[i]);
                    }
                }
            }
        }

        private void Find() {
            assemblyNames = new List<string>();
            typeNames = new List<string>();
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach(Type t in a.GetTypes()) {
                    if(className == t.Name) {
                        assemblyNames.Add(a.GetName().Name);
                        typeNames.Add(t.FullName);
                    }
                }
            }
        }

    }
}