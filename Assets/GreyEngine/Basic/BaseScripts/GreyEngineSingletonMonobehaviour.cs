using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GreyEngine.Basic {
    abstract public class GreyEngineSingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance {
            get {
                if(instance == null) {
                    Type t = typeof(T);
                    instance = (T)FindObjectOfType(t);
                    if(instance == null) {
                        Debug.LogError(t.FullName + " をアタッチしているGameObjectがシーン上に存在しません");
                    }
                }
                return instance;
            }
        }
        virtual protected void Awake() {
            if(this != Instance) {
                Destroy(this);
                Debug.LogError(typeof(T).FullName + " がシーン内で重複していたため、コンポーネントを破棄しました。");
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}