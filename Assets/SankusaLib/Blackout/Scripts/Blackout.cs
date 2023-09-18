using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace SankusaLib {
    public class Blackout : MonoBehaviour
    {
        /// <summary>
        /// 暗幕画像
        /// </summary>
        [SerializeField] Image blackoutImage;
        /// <summary>
        /// デフォルトフェード時間
        /// </summary>
        [SerializeField] float defaultDuration;
        /// <summary>
        /// フェード中にレイキャストターゲットをオンにする
        /// </summary>
        [SerializeField] bool raycastTargetSynchronizeAlpha = true;

        private bool isPlaying = false;
        public bool IsPlaying => isPlaying;

        private static Blackout instance;
        public static Blackout Instance {
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

        void Update() {
            if(raycastTargetSynchronizeAlpha) {
                if(blackoutImage.color.a == 0) {
                    blackoutImage.raycastTarget = false;
                } else {
                    blackoutImage.raycastTarget = true;
                }
            }
        }

        /// <summary>
        /// 透明度フェード
        /// </summary>
        /// <param name="toAlpha">終端透明度<param>
        /// <param name="duration">フェード時間<param>
        /// <param name="onComplete">フェード終了時コールバック<param>
        public void PlayFade(float toAlpha, float duration , Action onComplete = null) {
            InternalPlayFade(toAlpha, duration, onComplete);
        }
        /// <summary>
        /// 透明度フェード
        /// </summary>
        /// <param name="toAlpha">終端透明度<param>
        /// <param name="onComplete">フェード終了時コールバック<param>
        public void PlayFade(float toAlpha, Action onComplete = null) {
            PlayFade(toAlpha, defaultDuration, onComplete);
        }

        /// <summary>
        /// 暗転
        /// </summary>
        /// <param name="duration">透明度1へのフェード時間及び透明度0へのフェード時間<param>
        /// <param name="onBlackout">暗転時コールバック<param>
        public void PlayBlackout(float duration, Action onBlackout = null) {
            PlayFade(1f, duration, () => {
                onBlackout?.Invoke();
                PlayFade(0, duration);
            });
        }

        /// <summary>
        /// 暗転
        /// </summary>
        /// <param name="onBlackout">暗転時コールバック<param>
        public void PlayBlackout(Action onBlackout = null) {
            PlayBlackout(defaultDuration, onBlackout);
        }

        private void InternalPlayFade(float toAlpha, float duration , Action onComplete = null) {
            isPlaying = true;
            blackoutImage
                .DOFade(toAlpha, duration)
                .OnComplete(() => {
                    isPlaying = false;
                    onComplete?.Invoke();
                }).SetLink(gameObject);
        }
    }
}