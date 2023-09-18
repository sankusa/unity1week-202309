using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using System.IO;

namespace SankusaLib.SoundLib {
    public class ScriptGenerator
    {
        private const string VOLUME_KEY_TEMPLATE = @"
namespace SankusaLib.SoundLib {
    public enum VolumeKey {
#BODY#
    }
}
";

        private const string SOUND_ID_CLASS_TEMPLATE = @"
namespace SankusaLib.SoundLib {
    public class SoundId {
#BODY#
    }
}
";
        private const string SOUND_MANAGER_PARTIAL_TEMPLATE = @"
using System;
using UnityEngine.Events;
namespace SankusaLib.SoundLib {
    public partial class SoundManager {
#BODY#
    }
}
";
        private const string SOUND_MANAGER_ACCESSOR_TEMPLATE = @"
using System;
using UnityEngine;
using UnityEngine.Events;
namespace SankusaLib.SoundLib {
    public partial class SoundManagerAccessor : MonoBehaviour {
#BODY#
    }
}
";
        private const string SOUND_MANAGER_VOLUME_PARAGRAPH = @"
        public float #VOLUME_KEY#Volume {
            get => FindVolume(""#VOLUME_KEY#"").Value;
            set => FindVolume(""#VOLUME_KEY#"").Value = value;
        }
        public bool #VOLUME_KEY#Mute {
            get => FindVolume(""#VOLUME_KEY#"").Mute;
            set => FindVolume(""#VOLUME_KEY#"").Mute = value;
        }
        public IObservable<float> On#VOLUME_KEY#VolumeChanged => FindVolume(""#VOLUME_KEY#"").OnValueChanged;
        public IObservable<bool> On#VOLUME_KEY#MuteChanged => FindVolume(""#VOLUME_KEY#"").OnMuteChanged;
";
        private const string SOUND_MANAGER_PLAYER_PARAGRAPH = @"
        public void Play#PLAYER_KEY#(string soundId, bool loop) {
            FindPlayer(""#PLAYER_KEY#"").Play(soundId, loop);
        }
        public void Play#PLAYER_KEY#(string soundId) {
            FindPlayer(""#PLAYER_KEY#"").Play(soundId);
        }
        public void Stop#PLAYER_KEY#() {
            FindPlayer(""#PLAYER_KEY#"").Stop();
        }
        public void Stop#PLAYER_KEY#(string soundId) {
            FindPlayer(""#PLAYER_KEY#"").Stop(soundId);
        }
        public void Fade#PLAYER_KEY#(float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer(""#PLAYER_KEY#"").Fade(duration, from, to, onFadeFinish);
        }
        public void Fade#PLAYER_KEY#(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            FindPlayer(""#PLAYER_KEY#"").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, float duration, bool loop) {
            FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, duration, loop);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, float duration) {
            FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, duration);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, bool loop) {
            FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, loop);
        }
        public void FadeIn#PLAYER_KEY#(string soundId) {
            FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId);
        }
        public void FadeOut#PLAYER_KEY#(float duration) {
            FindPlayer(""#PLAYER_KEY#"").FadeOut(duration);
        }
        public void FadeOut#PLAYER_KEY#() {
            FindPlayer(""#PLAYER_KEY#"").FadeOut();
        }
        public void FadeOut#PLAYER_KEY#(string soundId, float duration) {
            FindPlayer(""#PLAYER_KEY#"").FadeOut(soundId, duration);
        }
        public void FadeOut#PLAYER_KEY#(string soundId) {
            FindPlayer(""#PLAYER_KEY#"").FadeOut(soundId);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, float duration, bool loop) {
            FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, duration, loop);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, float duration) {
            FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, duration);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, bool loop) {
            FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, loop);
        }
        public void CrossFade#PLAYER_KEY#(string soundId) {
            FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId);
        }
";
        private const string SOUND_MANAGER_ACCESSOR_VOLUME_PARAGRAPH = @"
        public float #VOLUME_KEY#Volume {
            get => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").Value;
            set => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").Value = value;
        }
        public bool #VOLUME_KEY#Mute {
            get => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").Mute;
            set => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").Mute = value;
        }
        public IObservable<float> On#VOLUME_KEY#VolumeChanged => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").OnValueChanged;
        public IObservable<bool> On#VOLUME_KEY#MuteChanged => SoundManager.Instance.FindVolume(""#VOLUME_KEY#"").OnMuteChanged;
";
        private const string SOUND_MANAGER_ACCESSOR_PLAYER_PARAGRAPH = @"
        public void Play#PLAYER_KEY#(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Play(soundId, loop);
        }
        public void Play#PLAYER_KEY#(string soundId) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Play(soundId);
        }
        public void Stop#PLAYER_KEY#() {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Stop();
        }
        public void Stop#PLAYER_KEY#(string soundId) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Stop(soundId);
        }
        public void Fade#PLAYER_KEY#(float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Fade(duration, from, to, onFadeFinish);
        }
        public void Fade#PLAYER_KEY#(string soundId, float duration, float from, float to, UnityAction onFadeFinish = null) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").Fade(soundId, duration, from, to, onFadeFinish);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, duration, loop);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, float duration) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, duration);
        }
        public void FadeIn#PLAYER_KEY#(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId, loop);
        }
        public void FadeIn#PLAYER_KEY#(string soundId) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeIn(soundId);
        }
        public void FadeOut#PLAYER_KEY#(float duration) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeOut(duration);
        }
        public void FadeOut#PLAYER_KEY#() {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeOut();
        }
        public void FadeOut#PLAYER_KEY#(string soundId, float duration) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeOut(soundId, duration);
        }
        public void FadeOut#PLAYER_KEY#(string soundId) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").FadeOut(soundId);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, float duration, bool loop) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, duration, loop);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, float duration) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, duration);
        }
        public void CrossFade#PLAYER_KEY#(string soundId, bool loop) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId, loop);
        }
        public void CrossFade#PLAYER_KEY#(string soundId) {
            SoundManager.Instance.FindPlayer(""#PLAYER_KEY#"").CrossFade(soundId);
        }
";
        private const string AUTO_GENERATED_SCRIPTS_PATH = "Assets/SankusaLib/SoundLib/AutoGeneratedScripts";

        public static void CreateVolumeKeyEnum(SoundManager soundManager) {
            string scriptBody = "";
            bool first = true;
            foreach(VolumeSetting setting in soundManager.VolumeSettings) {
                if(first) {
                    scriptBody += "        " + setting.Key;
                    first = false;
                } else {
                    scriptBody += ",\r\n        " + setting.Key;
                }
            }
            string script = VOLUME_KEY_TEMPLATE.Replace("#BODY#", scriptBody);

            string filePath = AUTO_GENERATED_SCRIPTS_PATH + "/VolumeKey.cs";
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }

        public static void CreateSoundIdClass() {
            string scriptBody = "";
            List<SoundDataContainer> containers = LoadAllAssets<SoundDataContainer>();
            foreach(SoundDataContainer container in containers) {
                foreach(SoundData data in container.SoundDataList) {
                    scriptBody += "        public const string " + data.Id + " = \"" + data.Id + "\";\r\n";
                }
            }
            string script = SOUND_ID_CLASS_TEMPLATE.Replace("#BODY#", scriptBody);

            string filePath = AUTO_GENERATED_SCRIPTS_PATH + "/SoundId.cs";
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }

        public static void CreateSoundManagerPartialClass(SoundManager soundManager) {
            string scriptBody = "";
            foreach(VolumeSetting setting in soundManager.VolumeSettings) {
                scriptBody += SOUND_MANAGER_VOLUME_PARAGRAPH.Replace("#VOLUME_KEY#", setting.Key);
            }
            foreach(SoundPlayerSetting setting in soundManager.SoundPlayerSettings) {
                scriptBody += SOUND_MANAGER_PLAYER_PARAGRAPH.Replace("#PLAYER_KEY#", setting.Key);
            }
            string script = SOUND_MANAGER_PARTIAL_TEMPLATE.Replace("#BODY#", scriptBody);
            string filePath = AUTO_GENERATED_SCRIPTS_PATH + "/SoundManagerPartial.cs";
            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }

        public static void CreateSoundManagerAccessor(SoundManager soundManager) {
            string scriptBody = "";
            foreach(VolumeSetting setting in soundManager.VolumeSettings) {
                scriptBody += SOUND_MANAGER_ACCESSOR_VOLUME_PARAGRAPH.Replace("#VOLUME_KEY#", setting.Key);
            }
            foreach(SoundPlayerSetting setting in soundManager.SoundPlayerSettings) {
                scriptBody += SOUND_MANAGER_ACCESSOR_PLAYER_PARAGRAPH.Replace("#PLAYER_KEY#", setting.Key);
            }
            string script = SOUND_MANAGER_ACCESSOR_TEMPLATE.Replace("#BODY#", scriptBody);
            string filePath = AUTO_GENERATED_SCRIPTS_PATH + "/SoundManagerAccessor.cs";
            File.WriteAllText(filePath, script);
            AssetDatabase.Refresh();
        }

        // static string GetCurrentDirectory()
        // {
        //     var flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        //     var asm = Assembly.Load("UnityEditor.dll");
        //     var typeProjectBrowser = asm.GetType("UnityEditor.ProjectBrowser");
        //     var projectBrowserWindow = EditorWindow.GetWindow(typeProjectBrowser);
        //     return (string)typeProjectBrowser.GetMethod("GetActiveFolderPath", flag).Invoke(projectBrowserWindow, null); 
        // }

        // // プロジェクト内の対象型のアセットを全てロード
        private static List<T> LoadAllAssets<T> () where T : Object {
            List<T> list = new List<T>(); 
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            foreach(string guid in guids) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                T asset = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                list.Add(asset);
            }
            return list;
        }
    }
}