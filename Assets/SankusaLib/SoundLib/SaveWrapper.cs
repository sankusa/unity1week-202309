using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public class SaveWrapper
    {
        public static void Save(string key, object obj)
        {
            if(obj.GetType().Equals(typeof(float))) {
                PlayerPrefs.SetFloat(key, (float)obj);
            } else if(obj.GetType().Equals(typeof(int))) {
                PlayerPrefs.SetInt(key, (int)obj);
            } else if(obj.GetType().Equals(typeof(string))) {
                PlayerPrefs.SetString(key, (string)obj);
            } else if(obj.GetType().Equals(typeof(bool))) {
                if((bool)obj == true) {
                    PlayerPrefs.SetInt(key, 1);
                } else {
                    PlayerPrefs.SetInt(key, 0);
                }
            } else {
                if(obj.GetType().IsPrimitive) {
                    Debug.LogError(obj.GetType().Name + " is unsupported type.");
                    return;
                } else {
                    PlayerPrefs.SetString(key, JsonUtility.ToJson(obj));
                }
            }
            
        }

        public static T Load<T>(string key)
        {
            if(typeof(T).Equals(typeof(float))) {
                return (T)(object)PlayerPrefs.GetFloat(key);
            } else if(typeof(T).Equals(typeof(int))) {
                return (T)(object)PlayerPrefs.GetInt(key);
            } else if(typeof(T).Equals(typeof(string))) {
                return (T)(object)PlayerPrefs.GetString(key);
            } else if(typeof(T).Equals(typeof(bool))) {
                int ret = PlayerPrefs.GetInt(key);
                if(ret == 1) {
                    return (T)(object)true;
                } else {
                    return (T)(object)false;
                }
            } else {
                if(typeof(T).IsPrimitive) {
                    Debug.LogError(typeof(T).Name + " is unsupported type.");
                    return default(T);
                } else {
                    string json = PlayerPrefs.GetString(key);
                    return JsonUtility.FromJson<T>(json);
                }
            }
        }

        public static bool HasKey(string key) {
            return PlayerPrefs.HasKey(key);
        }
    }
}