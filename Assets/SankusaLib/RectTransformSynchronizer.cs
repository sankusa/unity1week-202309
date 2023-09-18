using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways()]
    public class RectTransformSynchronizer : MonoBehaviour
    {
        [SerializeField] private RectTransform target;
        private RectTransform rectTransform;

        void Start() {
            rectTransform = GetComponent<RectTransform>();
        }

        void Update() {
            if(rectTransform != null && target != null) {
                rectTransform.anchorMin = target.anchorMin;
                rectTransform.anchorMax = target.anchorMax;
                rectTransform.pivot = target.pivot;
                rectTransform.anchoredPosition = target.anchoredPosition;
                // rectTransform.position = target.position;
                rectTransform.rotation = target.rotation;
                rectTransform.localScale = target.localScale;
                rectTransform.sizeDelta = target.sizeDelta;
            }
        }
    }
}