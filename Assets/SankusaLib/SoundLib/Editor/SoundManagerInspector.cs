using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace SankusaLib.SoundLib {
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerInspector : Editor
    {
        private bool isPlayMode = false;

        private ReorderableList volumeList;
        private ReorderableList playerList;
        private Dictionary<string, ReorderableList> volumeKeyListDictionary = new Dictionary<string, ReorderableList>();
        
        private SerializedProperty volumeSettingsProp;
        private SerializedProperty soundPlayerSettingsProp;

        void OnEnable() {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            UpdateIsPlayMode();
            Initialize();
        }

        void OnDisable() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state) {
            UpdateIsPlayMode();
        }

        private void Initialize() {
            volumeSettingsProp = serializedObject.FindProperty("volumeSettings");
            soundPlayerSettingsProp = serializedObject.FindProperty("soundPlayerSettings");

            volumeList = new ReorderableList(serializedObject, volumeSettingsProp);
            volumeList.drawHeaderCallback += (Rect rect) => EditorGUI.LabelField(rect, "Volume");

            volumeList.drawElementCallback += VolumeSettingsDrawElement;
            volumeList.elementHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;

            playerList = new ReorderableList(serializedObject, soundPlayerSettingsProp);
            playerList.drawHeaderCallback += (Rect rect) => EditorGUI.LabelField(rect, "SoundPlaer");
            playerList.drawElementCallback += SoundPlayerSettingsDrawElement;
            playerList.elementHeightCallback += SoundPlayerSettingsElementHeight;

            volumeKeyListDictionary.Clear();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Rect modeRect = EditorGUILayout.GetControlRect();
            if(isPlayMode) {
                EditorGUI.DrawRect(modeRect, new Color(0, 0.6f, 0.6f, 1));
                EditorGUI.LabelField(modeRect, "Play Mode");
            } else {
                EditorGUI.DrawRect(modeRect, Color.black);
                EditorGUI.LabelField(modeRect, "Setting Mode");
            }
            if(GUILayout.Button("スクリプト更新")) {
                ScriptGenerator.CreateVolumeKeyEnum(target as SoundManager);
                ScriptGenerator.CreateSoundManagerPartialClass(target as SoundManager);
                ScriptGenerator.CreateSoundManagerAccessor(target as SoundManager);
            }
            
            volumeList.DoLayoutList();

            playerList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private void VolumeSettingsDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty prop = volumeSettingsProp.GetArrayElementAtIndex(index);
            
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60;

            List<Rect> verticalDividedRects = RectUtil.DivideRectVertical(rect, new List<float>(){1, 1, 1}, null, 0, EditorGUIUtility.standardVerticalSpacing);

            EditorGUI.LabelField(verticalDividedRects[0], prop.FindPropertyRelative("key").stringValue + " Volume", GUI.skin.box);

            List<Rect> line1Rects = RectUtil.DivideRectHorizontal(verticalDividedRects[1], new List<float>(){1, 1}, null, EditorGUIUtility.standardVerticalSpacing);

            EditorGUI.BeginDisabledGroup(isPlayMode);
            EditorGUI.PropertyField(line1Rects[0], prop.FindPropertyRelative("key"));
            EditorGUI.PropertyField(line1Rects[1], prop.FindPropertyRelative("save"));
            EditorGUI.EndDisabledGroup();

            List<Rect> line2Rects = RectUtil.DivideRectHorizontal(verticalDividedRects[2], new List<float>(){1, 1}, null, EditorGUIUtility.standardVerticalSpacing);
            if(isPlayMode) {
                Volume volume = SoundManager.Instance.FindVolume(prop.FindPropertyRelative("key").stringValue);
                volume.Value = EditorGUI.Slider(line2Rects[0], ObjectNames.NicifyVariableName("Volume"), volume.Value, 0, 1);
                volume.Mute = EditorGUI.Toggle(line2Rects[1], ObjectNames.NicifyVariableName("Mute"), volume.Mute);
            } else {
                EditorGUI.PropertyField(line2Rects[0], prop.FindPropertyRelative("volume"));
                EditorGUI.PropertyField(line2Rects[1], prop.FindPropertyRelative("mute"));
            }


            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        private void SoundPlayerSettingsDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty prop = soundPlayerSettingsProp.GetArrayElementAtIndex(index);
            string listDictionaryKey = prop.displayName;

            List<Rect> rects = RectUtil.DivideRectVertical(rect, new List<float>(){22, 22, 1, 22}, new List<int>(){0, 1, 3}, 2);
            List<Rect> line1Rects = RectUtil.DivideRectHorizontal(rects[0], new List<float>(){1, 1}, null, 2);
            List<Rect> line2Rects = RectUtil.DivideRectHorizontal(rects[1], new List<float>(){1, 1}, null, 2);
            List<Rect> line4Rects = RectUtil.DivideRectHorizontal(rects[3], new List<float>(){1, 1}, null, 2);

            // ディクショナリーから見つからなければReorderableList作成
            if(!volumeKeyListDictionary.ContainsKey(listDictionaryKey)) {
                volumeKeyListDictionary.Add(listDictionaryKey, new ReorderableList(prop.serializedObject, prop.FindPropertyRelative("volumeKeys")));

                volumeKeyListDictionary[listDictionaryKey].drawElementCallback += (pos, index, isActive, isFocused) => {
                    var volumeKeyProp = prop.FindPropertyRelative("volumeKeys").GetArrayElementAtIndex(index);

                    List<Rect> horizontalRects = RectUtil.DivideRectHorizontal(pos, new List<float>(){1, 1}, null, 2);

                    string[] volumeKeys = GetVolumeKeys();
                    int selectIndex = volumeKeys.ToList().IndexOf(volumeKeyProp.stringValue);
                    if(selectIndex == -1) selectIndex = 0;
                    selectIndex = EditorGUI.Popup(horizontalRects[0], selectIndex, volumeKeys);
                    if(volumeKeys[selectIndex] == "") {
                        EditorGUI.HelpBox(horizontalRects[1], "Choose volume key.", MessageType.Error);
                    }
                    volumeKeyProp.stringValue = volumeKeys[selectIndex];
                };

                volumeKeyListDictionary[listDictionaryKey].drawHeaderCallback += (pos) => {
                    EditorGUI.LabelField(pos, "VolumeKeys");
                };
            }

            float originalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;

            EditorGUI.PropertyField(line1Rects[0], prop.FindPropertyRelative("key"));
            EditorGUI.PropertyField(line1Rects[1], prop.FindPropertyRelative("playerCount"));
            EditorGUI.PropertyField(line2Rects[0], prop.FindPropertyRelative("defaultLoop"));
            EditorGUI.PropertyField(line2Rects[1], prop.FindPropertyRelative("defaultFadeDuration"));

            volumeKeyListDictionary[listDictionaryKey].DoList(rects[2]);

            EditorGUI.PropertyField(line4Rects[0], prop.FindPropertyRelative("logCapacity"));

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        public float SoundPlayerSettingsElementHeight(int index)
        {
            var containerSettingProp = soundPlayerSettingsProp.GetArrayElementAtIndex(index);
            var volumeKeysProp = containerSettingProp.FindPropertyRelative("volumeKeys");
            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            float listElementHeight = 23;
            float listOtherHeight = 60;

            float listHeight = (volumeKeysProp.arraySize == 0 ? 1 : volumeKeysProp.arraySize) * listElementHeight + listOtherHeight;
            return listHeight + lineHeight * 3;
        }

        private void UpdateIsPlayMode() {
            // GameObjectがプレハブか判定
            SoundManager sm = (SoundManager)(serializedObject.targetObject);
            bool isPrefab = sm.gameObject.scene.name == null;
            // 再生中かつプレハブでない(ヒエラルキー上)ならisPlayModeをtrueに
            if(EditorApplication.isPlaying && !isPrefab) {
                isPlayMode = true;
            } else {
                isPlayMode = false;
            }
        }

        public string[] GetVolumeKeys() {
            List<string> keys = new List<string>();
            keys.Add("");

            for(int i = 0; i < volumeSettingsProp.arraySize; i++) {
                keys.Add(volumeSettingsProp.GetArrayElementAtIndex(i).FindPropertyRelative("key").stringValue);
            }
            return keys.ToArray();
        }
    }
}