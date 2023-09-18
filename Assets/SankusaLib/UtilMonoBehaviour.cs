using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SankusaLib {
    public class UtilMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnable = new UnityEvent();
        [SerializeField] private UnityEvent onStart = new UnityEvent();
        [SerializeField] private UnityEvent onDisable = new UnityEvent();
        [SerializeField] private UnityEvent onDestroy = new UnityEvent();

        void OnEnable() {
            onEnable.Invoke();
        }

        void Start() {
            onStart.Invoke();
        }

        void OnDisable() {
            onDisable.Invoke();
        }

        void OnDestroy() {
            onDestroy.Invoke();
        }

        public void Log(string message) {
            Debug.Log(gameObject.name + " : " + message);
        }
    }
}