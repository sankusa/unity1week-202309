using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    public class JsonSave
    {
        public static bool logOutput = false;

        public static void Save(string key, object obj) {
            string json = JsonUtility.ToJson(obj);
            if(logOutput) Debug.Log("Save => " + json);
            PlayerPrefs.SetString(key, JsonUtility.ToJson(obj));
            //PlayerPrefs.Save();
        }
        public static T Load<T>(string key) {
            string json = PlayerPrefs.GetString(key);
            if(logOutput) Debug.Log("Load => " + json);
            return JsonUtility.FromJson<T>(json);
        }
        public static void LoadOverWrite(string key, object objectToOverWrite) {
            string json = PlayerPrefs.GetString(key);
            if(logOutput) Debug.Log("LoadOverWrite => " + json);
            JsonUtility.FromJsonOverwrite(json, objectToOverWrite);
        }
    } 
}