using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [CreateAssetMenu(fileName = nameof(LoadingScreenGenerator), menuName = "SankusaLib/LoadingScreen/LoadingScreenGenerator")]
    public class LoadingScreenGenerator : ScriptableObject
    {
        [SerializeField] GameObject loadingScreenPrefab;
        [SerializeField] bool generateOnBeforeSceneLoad = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GenerateLoadingScreen() {
            // ResouorcesからLoadingScreenGeneratorのアセットファイルをロード
            LoadingScreenGenerator instance = Resources.Load<LoadingScreenGenerator>(nameof(LoadingScreenGenerator));
            if(instance == null) {
                Debug.LogError("\"Resources/LoadingScreenGenerator\" is nothing");
                return;
            }
            // プレハブをInstantiate
            if(instance.generateOnBeforeSceneLoad) {
                Instantiate(instance.loadingScreenPrefab);
            }
            // アセットファイルをアンロード
            Resources.UnloadAsset(instance);
        }
    }
}