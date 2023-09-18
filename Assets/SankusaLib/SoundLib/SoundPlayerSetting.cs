using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    [System.Serializable]
    public class SoundPlayerSetting
    {
        [SerializeField] private string key;
        public string Key => key;

        [SerializeField, Min(1)] private int playerCount;
        public int PlayerCount => playerCount;

        [SerializeField] private bool defaultLoop;
        public bool DefaultLoop => defaultLoop;

        [SerializeField] private float defaultFadeDuration;
        public float DefaultFadeDuration => defaultFadeDuration;

        [SerializeField, Min(-1)] private int logCapacity = 0;
        public int LogCapacity => logCapacity;

        [SerializeField, NoLabel] private List<string> volumeKeys;
        public IReadOnlyList<string> VolumeKeys => volumeKeys;
    }
}