using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SankusaLib {
    public class SimpleDialogBoxManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> dialogBoxSource;

        private static SimpleDialogBoxManager instance;
        public static SimpleDialogBoxManager Instance {
            get {
                if(instance) {
                    return instance;
                } else {
                    Debug.LogError(typeof(Blackout) + " is nothing");
                    return null;
                }
            }
        }

        void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void CallDialogBox(string message, string selection1 ,string selection2, UnityAction callback1 = null, UnityAction callback2 = null) {
            CallDialogBox(0, message, selection1, selection2, callback1, callback2);
        }
        public void CallDialogBox(int index, string message, string selection1 ,string selection2, UnityAction callback1 = null, UnityAction callback2 = null) {
            GameObject go = Instantiate(dialogBoxSource[index], this.transform);
            SimpleDialogBox dialogBox = go.GetComponent<SimpleDialogBox>();

            go.SetActive(true);
            dialogBox.SetUp(message, selection1, selection2, callback1, callback2);
        }
        public void CallDialogBox(string message, string selection1, UnityAction callback1 = null) {
            CallDialogBox(0, message, selection1, callback1);
        }
        public void CallDialogBox(int index, string message, string selection1, UnityAction callback1 = null) {
            GameObject go = Instantiate(dialogBoxSource[index], this.transform);
            SimpleDialogBox dialogBox = go.GetComponent<SimpleDialogBox>();

            go.SetActive(true);
            dialogBox.SetUp(message, selection1, callback1);
        }
    }
}