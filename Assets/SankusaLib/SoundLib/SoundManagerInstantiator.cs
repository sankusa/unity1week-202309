using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    [CreateAssetMenu(fileName = nameof(SoundManagerInstantiator), menuName = nameof(SankusaLib) + "/" + nameof(SoundLib) + "/" + nameof(SoundManagerInstantiator))]
    public class SoundManagerInstantiator : ScriptableObject
    {
        [SerializeField] GameObject soundManagerPrefab;
        [SerializeField] bool generateOnBeforeSceneLoad = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstantiateSoundManager() {
            // ResouorcesからSoundManagerInstantiatorをロード
            SoundManagerInstantiator instance = Resources.Load<SoundManagerInstantiator>(nameof(SoundManagerInstantiator));
            if(instance == null) {
                Debug.LogWarning("\"Resources/SoundManagerInstantiator\" is nothing");
                return;
            }
            // プレハブをInstantiate
            if(instance.generateOnBeforeSceneLoad) {
                Instantiate(instance.soundManagerPrefab);
            }
            // アセットファイルをアンロード
            Resources.UnloadAsset(instance);
        }
    }
}