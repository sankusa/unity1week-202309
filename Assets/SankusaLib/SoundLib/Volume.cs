using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace SankusaLib.SoundLib {
    public class Volume
    {
        public const string VOLUME_SAVE_KEY_SUFFIX = "_Volume_Value";
        public const string MUTE_SAVE_KEY_SUFFIX = "_Volume_Mute";

        private string key;
        public string Key => key;

        private bool save;
        public bool Save => save;

        private string volumeSaveKey;

        private string muteSaveKey;

        private FloatReactiveProperty value;
        public IObservable<float> OnValueChanged => value;

        public float Value {
            get => value.Value;
            set => this.value.Value = Mathf.Clamp01(value);
        }

        private void SaveVolume() {
            SaveWrapper.Save(volumeSaveKey, value.Value);
        }

        private void LoadVolume() {
            if(!SaveWrapper.HasKey(volumeSaveKey)) return;
            Value = SaveWrapper.Load<float>(volumeSaveKey);
        }

        private BoolReactiveProperty mute;
        public IObservable<bool> OnMuteChanged => mute;

        public bool Mute {
            get => mute.Value;
            set => mute.Value = value;
        }

        private void SaveMute() {
            SaveWrapper.Save(muteSaveKey, mute.Value);
        }

        private void LoadMute() {
            if(!SaveWrapper.HasKey(muteSaveKey)) return;
            SaveWrapper.Load<bool>(muteSaveKey);
        }

        public Volume(VolumeSetting setting, string saveKeyPrefix) {
            key = setting.Key;
            save = setting.Save;
            value = new FloatReactiveProperty(setting.Volume);
            mute = new BoolReactiveProperty(setting.Mute);
            volumeSaveKey = saveKeyPrefix + "_" + key + VOLUME_SAVE_KEY_SUFFIX;
            muteSaveKey = saveKeyPrefix + "_" + key + MUTE_SAVE_KEY_SUFFIX;

            if(save) {
                LoadVolume();
                LoadMute();

                OnValueChanged.Subscribe(_ => SaveVolume());
                OnMuteChanged.Subscribe(_ => SaveMute());
            }
        }
    }
}