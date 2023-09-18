using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    public class GameObjectDuplicator : MonoBehaviour
    {
        [SerializeField] private GameObject original;
        [SerializeField] private float duplicateInterval = 1f;
        [SerializeField] private bool activateOnInstantiate = false;

        IEnumerator Start() {
            while(true) {
                yield return new WaitForSeconds(duplicateInterval);
                GameObject go = Instantiate(original);
                if(activateOnInstantiate) go.SetActive(true);
            }
        }
    }
}