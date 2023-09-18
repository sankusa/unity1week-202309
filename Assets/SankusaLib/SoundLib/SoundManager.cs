using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public partial class SoundManager : MonoBehaviour
    {
        private static SoundManager instance;
        public static SoundManager Instance {
            get {
                if(instance) {
                    return instance;
                } else {
                    Debug.LogError(typeof(SoundManager) + "is nothing.");
                    return null;
                }
            }
        }


        [SerializeField] private string saveKeyPrefix;
        [SerializeField] private List<VolumeSetting> volumeSettings;
        public IReadOnlyList<VolumeSetting> VolumeSettings => volumeSettings;

        private List<Volume> volumeList = new List<Volume>();

        [SerializeField] private List<SoundPlayerSetting> soundPlayerSettings;
        public IReadOnlyList<SoundPlayerSetting> SoundPlayerSettings => soundPlayerSettings;

        private List<SoundPlayer> soundPlayers = new List<SoundPlayer>();
        public IReadOnlyList<SoundPlayer> SoundPlayers => soundPlayers;

        void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Debug.LogError("There are multiple " + typeof(SoundManager) + " in hierarchy.");
            }

            foreach(var setting in volumeSettings) {
                volumeList.Add(new Volume(setting, saveKeyPrefix));
            }
            foreach(var setting in soundPlayerSettings) {
                soundPlayers.Add(new SoundPlayer(gameObject, setting, volumeList));
            }
        }

        void Update() {
            foreach(var soundPlayer in soundPlayers) {
                soundPlayer.Update();
            }
        }

        public Volume FindVolume(string key) {
            return volumeList.Find(x => x.Key == key);
        }

        public SoundPlayer FindPlayer(string key) {
            return soundPlayers.Find(x => x.Key == key);
        }
    }
}