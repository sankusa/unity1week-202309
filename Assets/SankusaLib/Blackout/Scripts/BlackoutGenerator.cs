using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [CreateAssetMenu(fileName = nameof(BlackoutGenerator), menuName = "SankusaLib/Blackout/BlackoutGenerator")]
    public class BlackoutGenerator : ScriptableObject
    {
        [SerializeField] GameObject blackoutPrefab;
        [SerializeField] bool generateOnBeforeSceneLoad = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GenerateBlackout() {
            // ResouorcesからBlackoutGeneratorのアセットファイルをロード
            BlackoutGenerator instance = Resources.Load<BlackoutGenerator>(nameof(BlackoutGenerator));
            if(instance == null) {
                Debug.LogError("\"Resources/BlackoutGenerator\" is nothing");
                return;
            }
            // プレハブをInstantiate
            if(instance.generateOnBeforeSceneLoad) {
                Instantiate(instance.blackoutPrefab);
            }
            // アセットファイルをアンロード
            Resources.UnloadAsset(instance);
        }
    }
}