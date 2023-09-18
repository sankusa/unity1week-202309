using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public class RectTransformSizeAdjuster : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Vector2 screenSize;
        private float cameraSize;
        [SerializeField] private bool followMainCamera = true;

        void Start() {
            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
            screenSize = new Vector2(Screen.width, Screen.height);
            cameraSize = Camera.main.orthographicSize;
        }

        void Update() {
            if(rectTransform != null && Camera.main != null) {
                if(Screen.width != screenSize.x || Screen.height != screenSize.y || Camera.main.orthographicSize != cameraSize) {
                    screenSize = new Vector2(Screen.width, Screen.height);
                    cameraSize = Camera.main.orthographicSize;
                    rectTransform.sizeDelta = new Vector2(screenSize.x, screenSize.y) * (cameraSize * 2 / screenSize.y);
                }

                if(followMainCamera && (Vector2)rectTransform.position != (Vector2)Camera.main.transform.position) {
                    rectTransform.position = (Vector2) Camera.main.transform.position;
                }
            }
        }
    }
}