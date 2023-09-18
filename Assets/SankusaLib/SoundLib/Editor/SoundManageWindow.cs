using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SankusaLib.SoundLib {
    public class SoundManageWindow : EditorWindow
    {
        private static SerializedObject serializedSoundDataMaster;
        private static SoundDataMaster soundDataMaster;

        private static AudioSource audioSource;
        private SoundData currentSoundData;

        private SerializedObject serializedSoundDataContainer;
        private SoundDataContainer soundDataContainer;

        private ReorderableList soundList;
        private Vector2 scrollPos = Vector2.zero;

        private float soundStartTime = 0;
        private float soundEndTime = float.MaxValue;

        private bool loop = false;

        private bool importSettingOpen = false;

        [MenuItem("SankusaLib/" + nameof(SoundManageWindow))]
        private static void Open() {
            GetWindow<SoundManageWindow>();
        }

        void OnEnable() {
            // SoundDataMasterをロード
            IReadOnlyList<string> soundDataMasterPaths = GetAssetPaths<SoundDataMaster>();
            if(soundDataMasterPaths.Count == 0) {
                Debug.LogError(nameof(SoundDataMaster) + " is nothing.");
                Close();
            } else if(soundDataMasterPaths.Count > 1) {
                Debug.LogError("There are multiple " + nameof(SoundDataMaster));
                Close();
            }
            soundDataMaster = AssetDatabase.LoadAssetAtPath<SoundDataMaster>(soundDataMasterPaths[0]);
            serializedSoundDataMaster = new SerializedObject(soundDataMaster);

            // 再生用AudioSourceを生成
            CreateAudioSourceObject();

            // 初期化
            Initialize();
        }

        void OnDisable() {
            DestroyAudioSourceObject();
        }

        private void Initialize() {
            if(soundDataContainer != null) {
                serializedSoundDataContainer = new SerializedObject(soundDataContainer);

                soundList = new ReorderableList(serializedSoundDataContainer, serializedSoundDataContainer.FindProperty("soundDataList"));
                soundList.drawHeaderCallback += (Rect rect) => EditorGUI.LabelField(rect, "音声データ一覧");
                soundList.drawElementCallback += OnDrawElement;
                soundList.elementHeightCallback += ElementHeight;
                soundList.onAddCallback += OnAdd;
                soundList.drawElementBackgroundCallback += DrawElementBackground;
            } else {
                serializedSoundDataContainer = null;
                soundList = null;
            }
        }

        void Update() {
            // SoundDataMasterの参照切れ対策
            for(int i = soundDataMaster.Containers.Count - 1; i >= 0; i--) {
                if(soundDataMaster.Containers[i] == null) {
                    soundDataMaster.Containers.RemoveAt(i);
                }
            }
            // 音量、ピッチの更新
            if(currentSoundData != null) {
                if(currentSoundData.VolumeType == VolumeType.Constant) {
                    audioSource.volume = currentSoundData.Volume;
                } else if(currentSoundData.VolumeType == VolumeType.Curve) {
                    audioSource.volume = Mathf.Clamp01(currentSoundData.VolumeCurve.Evaluate(audioSource.time));
                }
                audioSource.pitch = currentSoundData.Pitch;
            }
            // 再生終了判定
            if(audioSource.isPlaying) {
                if(audioSource.time >= soundEndTime) {
                    if(loop) {
                        audioSource.time = soundStartTime;
                    } else {
                        audioSource.Stop();
                    }
                }
            }

            if(audioSource.isPlaying) {
                Repaint();
            }
        }

        void OnGUI() {
            serializedSoundDataMaster.Update();

            // SoundDataContainerフィールド
            EditorGUI.BeginChangeCheck();
            soundDataContainer = (SoundDataContainer)EditorGUILayout.ObjectField(soundDataContainer, typeof(SoundDataContainer), false);
            if(EditorGUI.EndChangeCheck()) {
                Initialize();
            }

            if(serializedSoundDataContainer != null) {
                serializedSoundDataContainer.Update();

                var forceToMonoProp = serializedSoundDataContainer.FindProperty("forceToMono");
                var loadTypeProp = serializedSoundDataContainer.FindProperty("loadType");

                
                using(new EditorGUILayout.HorizontalScope()) {
                    // SoundDataMasterへのアタッチの操作
                    bool isAttachedToSoundDataMaster = (soundDataMaster.Containers.IndexOf(soundDataContainer) != -1);

                    EditorGUI.BeginChangeCheck();
                    isAttachedToSoundDataMaster = EditorGUILayout.Toggle("ゲーム開始前にロード", isAttachedToSoundDataMaster);
                    if(EditorGUI.EndChangeCheck()) {
                        var containersProp = serializedSoundDataMaster.FindProperty("containers");
                        if(isAttachedToSoundDataMaster) {
                            containersProp.InsertArrayElementAtIndex(containersProp.arraySize);
                            containersProp.GetArrayElementAtIndex(containersProp.arraySize - 1).objectReferenceValue = soundDataContainer;
                        } else {
                            for(int i = containersProp.arraySize - 1; i >= 0; i--) {
                                if(containersProp.GetArrayElementAtIndex(i).objectReferenceValue == soundDataContainer) {
                                    containersProp.DeleteArrayElementAtIndex(i);
                                    containersProp.DeleteArrayElementAtIndex(i);
                                }
                            }
                        }
                    }

                    // 自動生成スクリプト(SoundId)の更新
                    if(GUILayout.Button("Idクラスを更新")) {
                        ScriptGenerator.CreateSoundIdClass();
                    }
                }

                // インポート設定
                importSettingOpen = EditorGUILayout.BeginFoldoutHeaderGroup(importSettingOpen, "インポート設定");
                if(importSettingOpen) {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box)) {
                        EditorGUILayout.PropertyField(forceToMonoProp);
                        EditorGUILayout.PropertyField(loadTypeProp);

                        EditorGUILayout.BeginHorizontal();
                        if(GUILayout.Button("選択中の音声データに適用")) {
                            if(soundList.index >= 0) {
                                var soundDataProp = serializedSoundDataContainer.FindProperty("soundDataList").GetArrayElementAtIndex(soundList.index);
                                ApplyImportSetting((AudioClip)soundDataProp.FindPropertyRelative("clip").objectReferenceValue);
                            }
                        }
                        if(GUILayout.Button("一括適用")) {
                            ApplyImportSettingAll();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                // 音声の再生
                EditorGUI.BeginChangeCheck();
                float inputTime = EditorGUILayout.Slider(audioSource.time, 0, audioSource.clip?.length ?? 0);
                if(EditorGUI.EndChangeCheck()) {
                    audioSource.time = inputTime;
                }
                using (new EditorGUILayout.HorizontalScope()) {
                    loop = EditorGUILayout.Toggle("ループ", loop);
                    if(GUILayout.Button(audioSource.isPlaying ? "停止" : "再生")) {
                        if(!audioSource.isPlaying && soundList.index != -1) {
                            SoundData data = soundDataContainer.SoundDataList[soundList.index];
                            AudioClip clip = data.Clip;
                            if(clip != null) {
                                PlaySound(clip, data.Volume, data.Pitch);
                                soundStartTime = data.Start * clip.length;
                                soundEndTime = data.End * clip.length;
                                audioSource.time = soundStartTime;
                                currentSoundData = data;
                            }
                        } else {
                            StopSound();
                        }
                    }
                }
                
                // SoundDataList
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                soundList.DoLayoutList();
                EditorGUILayout.EndScrollView();
                
                serializedSoundDataContainer.ApplyModifiedProperties();
            }

            serializedSoundDataMaster.ApplyModifiedProperties();
        }

        // -------- ReorderableListコールバック --------

        private void OnAdd(ReorderableList list) {
            var soundDataListProp = serializedSoundDataContainer.FindProperty("soundDataList");
            soundDataListProp.InsertArrayElementAtIndex(soundDataListProp.arraySize);

            var soundDataProp = soundDataListProp.GetArrayElementAtIndex(soundDataListProp.arraySize - 1);
            // 追加されたSoundData(0埋め)を初期化
            soundDataProp.FindPropertyRelative("volume").floatValue = 1f;
            soundDataProp.FindPropertyRelative("pitch").floatValue = 1f;
            soundDataProp.FindPropertyRelative("end").floatValue = 1f;
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            var soundDataProp = serializedSoundDataContainer.FindProperty("soundDataList").GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, soundDataProp);
        }

        private float ElementHeight(int index) {
            return EditorGUI.GetPropertyHeight(soundList.serializedProperty.GetArrayElementAtIndex(index));
        }

        //背景の描画(GUI.backgroundColorを変更すると+や-ボタンなど、余計な所の色も変わってしまう)
        private void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused){
            if(index == -1) return;
            AudioClip clip = soundDataContainer.SoundDataList[index].Clip;
            string soundId = soundDataContainer.SoundDataList[index].Id;

            Texture2D tex = new Texture2D(1, 1);
            Color color = Color.black;
            color = new Color((index % 2), (index % 2), (index % 2), 0.1f);
            if(!EqualsToImportSetting(clip)) {
                color *= new Color(0.5f, 0.5f, 0.5f, 1);
                color += new Color(0.4f, 0, 0, 0.2f);
            }
            if(clip == null || soundId == "") {
                color = Color.black;
            }
            if(isFocused) {
                color *= new Color(0.5f, 0.5f, 0.5f, 1);
                color += new Color(0, 0, 0.4f, 0.2f);
            }
            tex.SetPixel(0, 0, color);
            tex.Apply();
            GUI.DrawTexture(rect, tex as Texture);
        }

        // -------- 音声再生 -------

        public void PlaySound(AudioClip clip, float volume, float pitch) {
            if(audioSource == null) return;

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        public void StopSound() {
            audioSource.Stop();
            currentSoundData = null;
        }

        // -------- インポート設定管理 --------

        private void ApplyImportSetting(AudioClip clip) {
            AudioImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as AudioImporter;
            importer.forceToMono = serializedSoundDataContainer.FindProperty("forceToMono").boolValue;
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            settings.loadType  = (AudioClipLoadType)System.Enum.ToObject(typeof(AudioClipLoadType), serializedSoundDataContainer.FindProperty("loadType").enumValueIndex);
            importer.defaultSampleSettings = settings;
            importer.SaveAndReimport();
        }

        private void ApplyImportSettingAll() {
            var soundDataListProp = serializedSoundDataContainer.FindProperty("soundDataList");
            for(int i = 0; i < soundDataListProp.arraySize; i++) {
                ApplyImportSetting((AudioClip)soundDataListProp.GetArrayElementAtIndex(i).FindPropertyRelative("clip").objectReferenceValue);
            }
        }

        private bool EqualsToImportSetting(AudioClip clip) {
            if(clip == null) return false;
            AudioImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(clip)) as AudioImporter;
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            if(importer.forceToMono == serializedSoundDataContainer.FindProperty("forceToMono").boolValue
            && settings.loadType == (AudioClipLoadType)System.Enum.ToObject(typeof(AudioClipLoadType), serializedSoundDataContainer.FindProperty("loadType").enumValueIndex)) {
                return true;
            } else {
                return false;
            }
        }

        // -------- AudioSource管理 --------

        private void CreateAudioSourceObject() {
            string goName = nameof(SoundManageWindow) + "_" + nameof(AudioSource);
            GameObject go = GameObject.Find(goName);
            if(go == null) {
                go = new GameObject(goName);
                go.hideFlags = HideFlags.HideAndDontSave;
            }

            audioSource = go.GetComponent<AudioSource>();
            if(audioSource == null) {
                audioSource = go.AddComponent<AudioSource>();
            }
        }

        private void DestroyAudioSourceObject() {
            if(audioSource != null) {
                DestroyImmediate(audioSource.gameObject);
            }
        }

        // -------- 汎用関数 --------

        // プロジェクト内の対象型のパスを全件取得
        private static List<string> GetAssetPaths<T> () where T : Object {
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