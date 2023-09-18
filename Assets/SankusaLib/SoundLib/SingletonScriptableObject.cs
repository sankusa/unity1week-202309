using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;
        public static T Instance {
            get {
                if(instance == null) {
                    instance = Resources.Load<T>(typeof(T).Name);
                    if(instance == null) {
                        Debug.LogError(typeof(T).Name + " is don't exist in Resources/");
                    }
                }
                return instance;
            }
        }
    }
}