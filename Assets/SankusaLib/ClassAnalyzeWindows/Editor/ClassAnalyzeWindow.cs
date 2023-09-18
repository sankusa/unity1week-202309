using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SankusaLib {
    public class ClassAnalyzeWindow : EditorWindow
    {
        List<Assembly> assemblies;
        List<string> assemblyNames;
        int assemblyIndex = 0;
        List<Type> types;
        List<string> typeNames;
        int typeIndex = 0;
        Type targetType;
        MethodInfo[] methods;
        // 条件フラグ
        bool isVisible = true;
        bool isPublic = true;
        // Flags
        bool showAccessibility = false;
        bool showBaseClass = false;
        bool showMethod = false;
        //
        Vector2 scrolPos = Vector2.zero;

        [MenuItem("SankusaLib/クラス解析")]
        private static void ShowWindow() {
            GetWindow<ClassAnalyzeWindow>("クラス解析");
        }

        void OnEnable() {
            GetAssemblys();
            GetTypes();
            targetType = types[0];
            methods = targetType.GetMethods();
        }

        void OnGUI() {
            scrolPos = EditorGUILayout.BeginScrollView(scrolPos);

            EditorGUI.BeginChangeCheck();
            assemblyIndex = EditorGUILayout.Popup(assemblyIndex, assemblyNames.ToArray());
            isVisible = EditorGUILayout.Toggle("IsVisible : ", isVisible);
            isPublic = EditorGUILayout.Toggle("isPublic : ", isPublic);
            // アセンブリが変更されていたらタイプを更新
            if(EditorGUI.EndChangeCheck()) {
                GetTypes();
            }

            EditorGUI.BeginChangeCheck();
            typeIndex = EditorGUILayout.Popup(typeIndex, typeNames.ToArray());
            if(EditorGUI.EndChangeCheck()) {
                // 解析情報
                targetType = types[typeIndex];
                methods = targetType.GetMethods();
            }


            // アクセシビリティ
            showAccessibility = EditorGUILayout.Foldout(showAccessibility, "アクセシビリティ");
            if(showAccessibility) {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Interface : " + targetType.IsInterface.ToString());
                EditorGUILayout.LabelField("Abstract : " + targetType.IsAbstract.ToString());
                EditorGUILayout.LabelField("Visible : " + targetType.IsVisible.ToString());
                EditorGUILayout.LabelField("Public : " + targetType.IsPublic.ToString());
                EditorGUILayout.LabelField("GenericType : " + targetType.IsGenericType.ToString());
                EditorGUI.indentLevel--;
            }
            // 派生情報
            showBaseClass = EditorGUILayout.Foldout(showBaseClass, "基底クラス");
            if(showBaseClass) {
                EditorGUI.indentLevel++;
                Type currentType = types[typeIndex];
                while(currentType != null) {
                    EditorGUILayout.LabelField(currentType.FullName);
                    currentType = currentType.BaseType;
                }
                EditorGUI.indentLevel--;
            }
            showMethod = EditorGUILayout.Foldout(showMethod, "メソッド");
            if(showMethod) {
                EditorGUI.indentLevel++;
                foreach(MethodInfo m in methods) {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
                        EditorGUILayout.LabelField("Public : " + m.IsPublic);
                        EditorGUILayout.LabelField("Static : " + m.IsStatic);
                        EditorGUILayout.LabelField("Type : " + m.ReturnType.ToString());
                        EditorGUILayout.LabelField("Name : " + m.Name);
                    }
                    
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndScrollView();
        }

        private void GetAssemblys() {
            // 全アセンブリを取得
            assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies()).OrderBy(value => {return value.GetName().Name;}).ToList();
            // アセンブリ名を取得
            assemblyNames = new List<string>();
            // 可視性を高めるため、記号入りの名称は弾く
            string removePattern = "<|`|\\+|_";
            for(int i = 0; i < assemblies.Count;) {
                string menuName = assemblies[i].GetName().Name.Replace(".", "/");
                if(Regex.IsMatch(menuName, removePattern) == false) {
                    assemblyNames.Add(menuName);
                    i++;
                } else {
                    assemblies.RemoveAt(i);
                }
            }
        }

        private void GetTypes() {
            typeIndex = 0;
            // アセンブリに属する全タイプを取得
            types = new List<Type>(assemblies[assemblyIndex].GetTypes()).Where(value => {return value.IsVisible == isVisible && value.IsPublic == isPublic;}).OrderBy(value => {return value.FullName;}).ToList();
            // タイプ名を取得
            typeNames = new List<string>();
            // 可視性を高めるため、記号入りの名称は弾く
            string removePattern = "<|`|\\+|_";
            for(int i = 0; i < types.Count;) {
                string menuName = types[i].FullName.Replace(".", "/");
                if(Regex.IsMatch(menuName, removePattern) == false) {
                    typeNames.Add(menuName);
                    i++;
                } else {
                    types.RemoveAt(i);
                }
            }
        }
    }
}