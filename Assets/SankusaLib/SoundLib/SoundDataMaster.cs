using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    [CreateAssetMenu(menuName = "SankusaLib/" + nameof(SoundDataMaster), fileName = nameof(SoundDataMaster))]
    public class SoundDataMaster : SingletonScriptableObject<SoundDataMaster>
    {
        [SerializeField] private List<SoundDataContainer> containers;
        public IList<SoundDataContainer> Containers => containers;

        public SoundData FindSoundData(string key) {
            foreach(SoundDataContainer container in containers) {
                SoundData soundData = container.FindSoundData(key);
                if(soundData != null) return soundData;
            }
            return null;
        }

        public void Add(IEnumerable<SoundDataContainer> containers) {
            foreach(SoundDataContainer container in containers) {
                if(!this.containers.Contains(container)) this.containers.Add(container);
            }
        }

        public void Remove(IEnumerable<SoundDataContainer> containers) {
            foreach(SoundDataContainer container in containers) {
                if(this.containers.Contains(container)) this.containers.Remove(container);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize() {
            SoundDataMaster tmp = Instance;
        }
    }
}