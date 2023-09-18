using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace SankusaLib.LeanLauncher
{
    public class LeanLauncherWindow : EditorWindow
    {
        private LeanLauncherData data;
        private SerializedObject serializedData;
        private SerializedProperty assetDataListProp;
        private Vector2 scrollPos;
        private static readonly float Margin = 2;

        // Selection.activeObject監視用。
        private Object activeObjectOld;

        [MenuItem("Tools/" + nameof(LeanLauncher) + " #X")]
        private static void Open()
        {
            if(HasOpenInstances<LeanLauncherWindow>())
            {
                GetWindow<LeanLauncherWindow>("", false).Close();
            }
            else
            {
                GetWindow<LeanLauncherWindow>("Lean Launcher");
            }
        }

        void OnEnable()
        {
            LoadData();
        }

        void Update()
        {
            if(Selection.activeObject != activeObjectOld)
            {
                Repaint();
            }

            activeObjectOld = Selection.activeObject;
        }

        void OnGUI()
        {
            // データをロード
            LoadData();
            if(data == null) return;

            Rect windowRect = position;
            Rect currentLineRect = new Rect(0, 0, windowRect.width, EditorGUIUtility.singleLineHeight);
            Rect remainingRect = new Rect(currentLineRect);
            GUIStyle buttonStyle = new GUIStyle("AppToolbarButtonLeft");

            serializedData.Update();

            // nullを削除
            for(int i = assetDataListProp.arraySize - 1; i >= 0; i--)
            {
                if(assetDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("asset").objectReferenceValue == null)
                {
                    assetDataListProp.DeleteArrayElementAtIndex(i);
                }
            }

            // スクロール開始
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            // アセット一覧表示
            System.Type assetTypeOld = null;
            for(int i = 0; i < assetDataListProp.arraySize; i++)
            {
                SerializedProperty assetProp = assetDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("asset");
                SerializedProperty labelProp = assetDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("label");
                Object asset = assetProp.objectReferenceValue;
                string label = labelProp.stringValue;

                // 表示内容取得
                Texture2D icon = AssetPreview.GetMiniThumbnail(asset);
                GUIContent nameLabelContent = null;
                if(label == "")
                {
                    nameLabelContent = new GUIContent(asset.name);
                }
                else
                {
                    nameLabelContent = new GUIContent(label);
                }

                // 表示内容の幅を取得
                Vector2 nameLabelSize = GUI.skin.label.CalcSize(nameLabelContent);
                float width = EditorGUIUtility.singleLineHeight + nameLabelSize.x + Margin;

                // 型が変わっていたら改行(可視性のため)
                if(assetTypeOld != null && asset.GetType() != assetTypeOld)
                {
                    currentLineRect.y += EditorGUIUtility.singleLineHeight + Margin;
                    remainingRect = new Rect(currentLineRect);
                }
                // 表示幅が足らなければ改行
                if(remainingRect.width < width && currentLineRect.width >= width)
                {
                    currentLineRect.y += EditorGUIUtility.singleLineHeight + Margin;
                    remainingRect = new Rect(currentLineRect);
                }

                // 各要素の表示領域を計算
                Rect rect = new Rect(remainingRect);
                rect.width = width - Margin;
                Rect buttonRect = new Rect(rect);
                Rect iconRect = new Rect(rect);
                iconRect.width = EditorGUIUtility.singleLineHeight;
                Rect nameRect = new Rect(rect);
                nameRect.xMin += EditorGUIUtility.singleLineHeight;

                // 要素を表示
                if(GUI.Button(buttonRect, GUIContent.none, buttonStyle))
                {
                    GUI.FocusControl("");
                    
                    if(Selection.activeObject == asset)
                    {
                        AssetDatabase.OpenAsset(asset);
                    }
                    else
                    {
                        Selection.activeObject = asset;
                    }
                }
                EditorGUI.LabelField(iconRect, new GUIContent(icon));
                EditorGUI.LabelField(nameRect, nameLabelContent);

                // 表示対象のアセットが選択中なら要素を青に
                if(Selection.activeObject == asset)
                {
                    EditorGUI.DrawRect(rect, new Color(0, 0, 1, 0.3f));
                }

                remainingRect.xMin += width;
                assetTypeOld = asset.GetType();
            }

            // 透明なBox。EditorGUIでの描画はEditorGUILayoutの自動レイアウトの枠組みに入っていないので、
            // EditorGUIで描画した分と同じ高さのBoxをEditorGUILayoutで描画して高さの帳尻を合わせる
            GUILayout.Box("", new GUIStyle("ToolbarSeachCancelButtonEmpty"), GUILayout.Height(currentLineRect.yMax));

            // スクロール終了
            EditorGUILayout.EndScrollView();

            // フッター(Selection.activeobjectがデータに存在する場合)
            int activeAssetDataIndex = -1;
            for(int i = 0; i < assetDataListProp.arraySize; i++)
            {
                if(assetDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("asset").objectReferenceValue == Selection.activeObject)
                {
                    activeAssetDataIndex = i;
                    break;
                }
            }

            if(activeAssetDataIndex != -1)
            {
                SerializedProperty labelProp = assetDataListProp.GetArrayElementAtIndex(activeAssetDataIndex).FindPropertyRelative("label");

                EditorGUI.BeginChangeCheck();

                Rect labelFieldRect = EditorGUILayout.GetControlRect();

                labelProp.stringValue = EditorGUI.TextField(labelFieldRect, labelProp.stringValue);
                if(labelProp.stringValue == "")
                {
                    EditorGUI.LabelField(labelFieldRect, "ラベルを入力", new GUIStyle("MeTimeLabel"));
                }
                if(GUILayout.Button("削除", buttonStyle))
                {
                    assetDataListProp.DeleteArrayElementAtIndex(activeAssetDataIndex);
                };
                
                if(EditorGUI.EndChangeCheck())
                {
                    Sort();
                }
            }

            // ドラッグ&ドロップ
            List<UnityEngine.Object> droppedObjects = DragAndDropUtil.GetObjects<UnityEngine.Object>(new Rect(0, 0, position.width, position.height));

            if(droppedObjects != null)
            {
                foreach(UnityEngine.Object asset in droppedObjects.Where(x => AssetDatabase.Contains(x)))
                {
                    bool exists = false;

                    for(int i = 0; i < assetDataListProp.arraySize; i++)
                    {
                        if(asset == assetDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("asset").objectReferenceValue)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if(!exists)
                    {
                        assetDataListProp.InsertArrayElementAtIndex(assetDataListProp.arraySize);
                        SerializedProperty newElementProp = assetDataListProp.GetArrayElementAtIndex(assetDataListProp.arraySize- 1);
                        // 値を設定
                        newElementProp.FindPropertyRelative("asset").objectReferenceValue = asset;
                        newElementProp.FindPropertyRelative("label").stringValue = "";

                        Sort();
                    }
                }
            }

            serializedData.ApplyModifiedProperties();
        }

        private void LoadData()
        {
            if(data == null)
            {
                data = LoadAsset<LeanLauncherData>();

                if(data == null) Debug.LogWarning("LeanLauncherData not found.");
            }

            if(serializedData == null)
            {
                if(data != null)
                {
                    serializedData = new SerializedObject(data);
                    assetDataListProp = serializedData.FindProperty("assetDataList");
                }
            }
        }

        private static T LoadAsset<T> () where T : UnityEngine.Object {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            if(guids.Length == 0) return default(T);

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            T asset = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));

            return asset;
        }

        private void Sort()
        {
            for(int i = assetDataListProp.arraySize - 2; i >= 0; i--)
            {
                for(int j = 0; j <= i; j++)
                {
                    SerializedProperty assetDataPropA = assetDataListProp.GetArrayElementAtIndex(j);
                    SerializedProperty assetDataPropB = assetDataListProp.GetArrayElementAtIndex(j + 1);
                    Object assetA = assetDataPropA.FindPropertyRelative("asset").objectReferenceValue;
                    Object assetB = assetDataPropB.FindPropertyRelative("asset").objectReferenceValue;
                    string typeNameA = assetA.GetType().FullName;
                    string typeNameB = assetB.GetType().FullName;
                    int typeCompare = typeNameA.CompareTo(typeNameB);
                    if(typeCompare > 0)
                    {
                        assetDataListProp.MoveArrayElement(j, j + 1);
                    }
                    else if(typeCompare == 0)
                    {
                        SerializedProperty labelPropA = assetDataPropA.FindPropertyRelative("label");
                        SerializedProperty labelPropB = assetDataPropB.FindPropertyRelative("label");
                        string labelA = labelPropA.stringValue != "" ? labelPropA.stringValue : assetA.name;
                        string labelB = labelPropB.stringValue != "" ? labelPropB.stringValue : assetB.name;
                        int labelCompare = labelA.CompareTo(labelB);
                        if(labelCompare > 0)
                        {
                            assetDataListProp.MoveArrayElement(j, j + 1);
                        }
                    }
                }
            }
        }
    }
}