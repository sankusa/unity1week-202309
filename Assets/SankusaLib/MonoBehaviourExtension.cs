using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SankusaLib {
    public static class MonoBehaviourExtension
    {
        public static IEnumerator DelayCoroutine(this MonoBehaviour behaviour, float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        public static Coroutine StartDelayCoroutine(this MonoBehaviour behaviour, float delay, Action action) {
            return behaviour.StartCoroutine(behaviour.DelayCoroutine(delay, action));
        }
    }
}