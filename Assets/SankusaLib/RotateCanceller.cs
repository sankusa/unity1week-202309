using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [ExecuteAlways()]
    public class RotateCanceller : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = Vector3.zero;
        void Update() {
            transform.rotation = Quaternion.Euler(offset);
        }
    }
}