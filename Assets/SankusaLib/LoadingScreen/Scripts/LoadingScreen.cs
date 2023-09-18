using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;

namespace SankusaLib {
    public class LoadingScreen : MonoBehaviour
    {
        private AsyncOperation asyncOperation;
        public float Progress => progress;
        private float progress = 0;
        private float progressOld = -1;
        [SerializeField] GameObject root;
        [SerializeField] float fakeLoadingTime = 0;
        public UnityEvent<float> onProgressValueChanged = new UnityEvent<float>();
        public UnityEvent onLoadStart = new UnityEvent();
        public UnityEvent onLoadEnd = new UnityEvent();

        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private static LoadingScreen instance;
        public static LoadingScreen Instance {
            get {
                if(instance) {
                    return instance;
                } else {
                    Debug.LogError(typeof(LoadingScreen) + " is nothing");
                    return null;
                }
            }
        }

        void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            onLoadStart.AddListener(() => OnLoadStart());
            onLoadEnd.AddListener(() => OnLoadEnd());

            root.SetActive(false);
        }

        void Update() {
            if(progress != progressOld) {
                onProgressValueChanged.Invoke(progress);
                progressOld = progress;
            }
        }

        // ロード
        public void LoadScene(string sceneName, Action action = null) {
            sw.Restart();
            onLoadStart?.Invoke();
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }
        IEnumerator LoadSceneCoroutine(string sceneName) {
            // ロード開始
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            // 自動でシーンがアクティブになる設定をオフ(isDoneもtrueにならなくなる)
            asyncOperation.allowSceneActivation = false;

            while(!asyncOperation.isDone) {
                progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                // FakeProgress計算
                float fakeProgress = 0;
                if(fakeLoadingTime > 0) {
                    fakeProgress = Mathf.Clamp01((float)sw.Elapsed.TotalSeconds / fakeLoadingTime);
                }
                // ProgressがFakeより進んでいたらFakeを使う
                progress = progress > fakeProgress ? fakeProgress : progress;
                // Progressが1になったらシーンをアクティブに(isDOneがtrueになる)
                if(progress == 1) {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
            sw.Stop();
            onLoadEnd.Invoke();
        }

        // ロード開始、終了時のデフォルト挙動
        protected virtual void OnLoadStart() {
            root.SetActive(true);
        }
        protected virtual void OnLoadEnd() {
            root.SetActive(false);
        }
    }
}