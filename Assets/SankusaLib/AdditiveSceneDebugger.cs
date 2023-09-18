using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace SankusaLib {
    [DefaultExecutionOrder(-1)]
    public class AdditiveSceneDebugger : MonoBehaviour
    {
        [SerializeField] private List<GameObject> inactivateObjectsOnAdditionalLoad;

        void Awake() {
            if(SceneManager.GetSceneAt(0) != gameObject.scene) {
                foreach(var go in inactivateObjectsOnAdditionalLoad) {
                    go.SetActive(false);
                }
            }
        }
    }
}