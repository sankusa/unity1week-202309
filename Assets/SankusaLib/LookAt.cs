using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    public class LookAt : MonoBehaviour
    {
        [SerializeField] private bool toCamera;
        [SerializeField] private bool ignoreX = false;
        [SerializeField] private bool ignoreY = false;
        [SerializeField] private bool ignoreZ = false;

        void Update() {
            Transform target = null;
            if(Camera.main != null) target = Camera.main.transform;

            if(target != null) {
                transform.LookAt(new Vector3(ignoreX ? transform.position.x : target.position.x, ignoreY ? transform.position.y : target.position.y, ignoreZ ? transform.position.z : target.position.z));
            }
            
        }
    }
}