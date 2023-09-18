using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    [System.Serializable]
    public class SoundData
    {
        [SerializeField] private string id;
        public string Id => id;

        [SerializeField] private AudioClip clip;
        public AudioClip Clip => clip;

        [SerializeField] private VolumeType volumeType = VolumeType.Constant;
        public VolumeType VolumeType => volumeType;

        [SerializeField, Range(0, 1)] private float volume = 1f;
        public float Volume => volume;

        [SerializeField] private AnimationCurve volumeCurve = new AnimationCurve();
        public AnimationCurve VolumeCurve => volumeCurve;

        [SerializeField] private float pitch = 1f;
        public float Pitch => pitch;

        [SerializeField] private float start = 0;
        public float Start => start;

        [SerializeField] private float end = 1;
        public float End => end;

        public SoundData() {
            id = "";
            volume = 1f;
            pitch = 1f;
        }
    }
}