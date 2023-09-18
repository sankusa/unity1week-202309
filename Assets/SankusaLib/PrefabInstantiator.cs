using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [CreateAssetMenu(fileName = nameof(PrefabInstantiator), menuName = "SankusaLib/PrefabInstantiator")]
    public class PrefabInstantiator : ScriptableObject
    {
        [SerializeField] List<GameObject> beforeSceneLoad;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InstantiateBeforeSceneLoad() {
            PrefabInstantiator instance = Resources.Load<PrefabInstantiator>(nameof(PrefabInstantiator));
            if(instance == null) {
                return;
            }

            foreach(GameObject prefab in instance.beforeSceneLoad) {
                if(prefab != null) {
                    GameObject go = Instantiate(prefab);
                    DontDestroyOnLoad(go);
                }
            }

            //Resources.UnloadAsset(instance);
        }
    }
}