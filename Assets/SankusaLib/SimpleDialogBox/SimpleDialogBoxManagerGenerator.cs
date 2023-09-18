using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [CreateAssetMenu(fileName = nameof(SimpleDialogBoxManagerGenerator), menuName = "SankusaLib/SimpleDialogBox/SimpleDialogBoxManagerGenerator")]
    public class SimpleDialogBoxManagerGenerator : ScriptableObject
    {
        [SerializeField] GameObject simpleDialogBoxManagerPrefab;
        [SerializeField] bool generateOnBeforeSceneLoad = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GenerateSimpleDialogBoxManager() {
            // ResouorcesからSimpleDialogBoxManagerGeneratorのアセットファイルをロード
            SimpleDialogBoxManagerGenerator instance = Resources.Load<SimpleDialogBoxManagerGenerator>(nameof(SimpleDialogBoxManagerGenerator));
            if(instance == null) {
                Debug.LogError("\"Resources/SimpleDialogBoxManagerGenerator\" is nothing");
                return;
            }
            // プレハブをInstantiate
            if(instance.generateOnBeforeSceneLoad) {
                Instantiate(instance.simpleDialogBoxManagerPrefab);
            }
            // アセットファイルをアンロード
            Resources.UnloadAsset(instance);
        }
    }
}