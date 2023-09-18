using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SankusaLib.SoundLib {
    [Serializable]
    public class VolumeSetting
    {
        [SerializeField] private string key;
        public string Key => key;

        [SerializeField] private bool save = false;
        public bool Save => save;

        [SerializeField, Range(0, 1)] private float volume = 1f;
        public float Volume => volume;

        [SerializeField] private bool mute = false;
        public bool Mute => mute;

        public VolumeSetting(string key, bool save, float volume, bool mute) {
            this.key = key;
            this.save = save;
            this.volume = volume;
            this.mute = mute;
        }
    }
}