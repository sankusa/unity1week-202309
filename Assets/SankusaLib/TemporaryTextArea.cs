using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SankusaLib {
    public class TemporaryTextArea : MonoBehaviour
    {
        [SerializeField] private LayoutGroup layoutGroup;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private float textLifeTime;
        [SerializeField] private int textCapacity = 10;

        public void ShowMessage(string message) {
            GameObject go = Instantiate(textPrefab, layoutGroup.transform);
            go.GetComponent<Text>().text = message;
            if(layoutGroup.transform.childCount > textCapacity) {
                for(int i = 0; i < layoutGroup.transform.childCount - textCapacity; i++) {
                    Destroy(layoutGroup.transform.GetChild(0).gameObject);
                }
            }
            StartCoroutine(DelayCoroutine(textLifeTime, () =>{if(go != null) Destroy(go);}));
        }

        private IEnumerator DelayCoroutine(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}