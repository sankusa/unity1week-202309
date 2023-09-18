using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace SankusaLib.ExpTableLib {
    [CustomEditor(typeof(ExpTable))]
    public class ExpTableInspector : Editor
    {
        private ReorderableList list;
        private SerializedProperty defaultLevelProp;
        private SerializedProperty expListProp;
        private ExpTable expTable;

        // ReorderableList要素内の表示幅の比率、固定設定
        private static readonly List<float> widthDivideRateList = new List<float>{60, 1, 1};
        private static readonly List<int> notExpandWidthIndexList = new List<int>{0};

        // 自動生成用変数
        long increment = 0;
        float increaseRate = 1f;
        int maxLevel = 1;

        Vector2 scrollPos = Vector2.zero;

        public void OnEnable() {
            expTable = target as ExpTable;

            defaultLevelProp = serializedObject.FindProperty("defaultLevel");
            expListProp = serializedObject.FindProperty("requiredExpList");
            list = new ReorderableList(serializedObject, expListProp);
            list.drawElementCallback += DrawElement;
            list.drawHeaderCallback += DrawHeader;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.PropertyField(defaultLevelProp, new GUIContent("初期レベル"));

            list.DoLayoutList();

            GUILayout.Label("経験値自動生成", GUI.skin.box);
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box)) {
                float originalLabelWidth = EditorGUIUtility.labelWidth;
                float originalFieldWidth = EditorGUIUtility.fieldWidth;

                EditorGUIUtility.labelWidth = 60;
                EditorGUIUtility.fieldWidth = 20;

                maxLevel = EditorGUILayout.IntField("最大レベル", maxLevel);

                EditorGUIUtility.labelWidth = 40;
                increment = EditorGUILayout.LongField("増加量", increment);
                increaseRate = EditorGUILayout.FloatField("増加率", increaseRate);

                EditorGUIUtility.labelWidth = originalLabelWidth;
                EditorGUIUtility.fieldWidth = originalFieldWidth;

                if(GUILayout.Button("生成")) {
                    //if(expListProp.arraySize > 1) {
                        // 経験値を生成
                        for(int i = 0; i < maxLevel; i++) {
                            // 足りない分の要素は生成
                            if(i >= expListProp.arraySize) {
                                expListProp.InsertArrayElementAtIndex(i);
                                if(i == 0) expListProp.GetArrayElementAtIndex(0).longValue = 1;
                            }
                            if(i != 0) expListProp.GetArrayElementAtIndex(i).longValue = (long)(expListProp.GetArrayElementAtIndex(i - 1).longValue * increaseRate) + increment;
                        }
                        // 過剰分を削除
                        for(int i = expListProp.arraySize - 1; i >= 0; i--) {
                            if(i >= maxLevel) expListProp.DeleteArrayElementAtIndex(i);
                        }
                    //}
                }
            }

            EditorGUILayout.EndScrollView();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader(Rect rect) {
            List<Rect> rects = RectUtil.DivideRect(RectUtil.HorizontalMargin(rect, 16), widthDivideRateList, notExpandWidthIndexList, 2, 2);

            EditorGUI.LabelField(rects[0], "レベル");
            EditorGUI.LabelField(rects[1], "レベルアップに必要な経験値");
            EditorGUI.LabelField(rects[2], "ここまでの累計経験値");
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty expProp = expListProp.GetArrayElementAtIndex(index);
            List<Rect> rects = RectUtil.DivideRect(rect, widthDivideRateList, notExpandWidthIndexList, 2, 2);

            EditorGUI.LabelField(rects[0], "Lv." + (index + defaultLevelProp.intValue).ToString());

            expProp.longValue = EditorGUI.LongField(rects[1], expProp.longValue);
            if(expProp.longValue < 1) expProp.longValue = 1;

            EditorGUI.LabelField(rects[2], expTable.LevelToTotalExp(index + defaultLevelProp.intValue).ToString());
        }
    }
}