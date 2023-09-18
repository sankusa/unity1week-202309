using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GreyEngine.Basic.TypeConversion {
    public class TypeConverterCheckWindow : EditorWindow
    {
        //private MasterTypeConverter masterConverter;
        private List<string> valueStrings;
        private Vector2 scrollPos = new Vector2(0, 0);

        [MenuItem("Tools/GreyEngine/Display/ConvertibleTypeInformation")]
        private static void ShowWindow() {
            var window = GetWindow<TypeConverterCheckWindow>("ConvertibleTypeInformation");
        }

        void OnEnable() {
            //masterConverter = new MasterTypeConverter();
            valueStrings = new List<string>();
            foreach(TypeConverter converter in UtilsForEditor.MasterConverter.converters) {
                valueStrings.Add(converter.InitialString);
            }
        }

        void OnGUI() {
            // スクロール開始
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            // 型情報表示
            for(int i = 0; i < UtilsForEditor.MasterConverter.converters.Count; i++) {
                // 対象のぺージ取得
                TypeConverter converter = UtilsForEditor.MasterConverter.converters[i];
                // 型名、名前空間付き型名
                EditorGUILayout.LabelField(converter.SimpleTypeName + " (" + converter.Type.FullName + ")");
                // 入力フィールド表示
                valueStrings[i] = converter.FieldLayout("入力フィールド", valueStrings[i]);
                // 内部値表示(編集不可)
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("内部値", converter.ValueToString(converter.StringToValue(valueStrings[i])));
                EditorGUI.EndDisabledGroup();
                // 区切り線
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            }
            // スクロール終了
            EditorGUILayout.EndScrollView();
        }
    }
}