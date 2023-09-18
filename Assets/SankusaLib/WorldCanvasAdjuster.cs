using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib {
    [RequireComponent(typeof(Canvas))]
    [ExecuteAlways]
    public class WorldCanvasAdjuster : MonoBehaviour
    {
        private Camera targetCamera = null;
        public Camera TargetCamera {
            get => targetCamera;
            set => targetCamera = value;
        }

        private RectTransform rectTransform;
        private Vector2 screenSize;
        private float cameraSize;
        [SerializeField] private float baseHeight = 1080f;
        private float baseHeightOld = 1080f;
        [SerializeField] private bool followMainCamera = true;
        [SerializeField] private float distance = 1000f;
        private float distanceOld = 0;
        private float fovOld = 0;

        void Start() {
            if(targetCamera == null) targetCamera = Camera.main;

            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
            screenSize = new Vector2(Screen.width, Screen.height);
            cameraSize = targetCamera.orthographicSize;

            if(targetCamera.orthographic) {
                AdjustSize_Orthographic();
                if(followMainCamera) FollowMainCamera_Orthographic();
            } else {
                AdjustSize_Perspective();
                if(followMainCamera) FollowMainCamera_Perspective();
            }
        }

        void Update() {
            if(targetCamera != null) {
                // 平行投影
                if(targetCamera.orthographic) {
                    if(Screen.width != screenSize.x || Screen.height != screenSize.y || targetCamera.orthographicSize != cameraSize || baseHeight != baseHeightOld) {
                        AdjustSize_Orthographic();
                    }

                    if(followMainCamera && (Vector2)transform.position != (Vector2)targetCamera.transform.position) {
                        FollowMainCamera_Orthographic();
                    }
                // 透視投影
                } else {
                    if(Screen.width != screenSize.x || Screen.height != screenSize.y || baseHeight != baseHeightOld || distanceOld != distance || fovOld != targetCamera.fieldOfView) {
                        AdjustSize_Perspective();
                    }
                    if(followMainCamera) {
                        FollowMainCamera_Perspective();
                    }
                }
            }
            baseHeightOld = baseHeight;
            distanceOld = distance;
            if(targetCamera != null) fovOld = targetCamera.fieldOfView;
        }

        private void AdjustSize_Orthographic() {
            if(rectTransform != null && targetCamera != null && screenSize.y > 0 && baseHeight > 0) {
                screenSize = new Vector2(Screen.width, Screen.height);
                cameraSize = Camera.main.orthographicSize;
                rectTransform.sizeDelta = new Vector2(screenSize.x / screenSize.y * baseHeight, baseHeight);
                rectTransform.localScale = Vector3.one * (cameraSize * 2 / baseHeight);
            }
        }

        private void FollowMainCamera_Orthographic() {
            if(targetCamera != null) {
                transform.position = (Vector2) targetCamera.transform.position;
            }
        }

        private void AdjustSize_Perspective() {
            if(rectTransform != null && targetCamera != null && screenSize.y > 0 && baseHeight > 0) {
                screenSize = new Vector2(Screen.width, Screen.height);
                rectTransform.sizeDelta = new Vector2(screenSize.x / screenSize.y * baseHeight, baseHeight);
                rectTransform.localScale = Vector3.one * ((distance * Mathf.Tan(2f * Mathf.PI * targetCamera.fieldOfView / 360f / 2f) * 2f) / baseHeight);
            }
        }

        private void FollowMainCamera_Perspective() {
            if(targetCamera != null) {
                transform.position = targetCamera.transform.position + targetCamera.transform.forward * distance;
                transform.rotation = targetCamera.transform.rotation;
            }
        }
    }
}