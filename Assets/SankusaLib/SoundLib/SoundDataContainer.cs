using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    [CreateAssetMenu(menuName = "SankusaLib/" + nameof(SoundDataContainer), fileName = nameof(SoundDataContainer))]
    public class SoundDataContainer : ScriptableObject
    {
        [SerializeField] private bool forceToMono = false;
        public bool ForceToMono => forceToMono;
        
        [SerializeField] private AudioClipLoadType loadType;
        public AudioClipLoadType LoadType => loadType;

        [SerializeField] private List<SoundData> soundDataList;
        public IReadOnlyList<SoundData> SoundDataList => soundDataList;

        public SoundData FindSoundData(string soundId) {
            return soundDataList.Find(soundData => soundData.Id == soundId);
        }
    }
}