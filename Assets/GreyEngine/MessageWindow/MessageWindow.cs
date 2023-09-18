using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GreyEngine.MessageWindow {
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField] private GameObject rootObject = null;
        [SerializeField] private Text speakerText = null;
        public Text SpeakerText => speakerText;
        [SerializeField] private Text messageText = null;
        [SerializeField, Header("1文字の表示にかかる時間")] float timePerChar = 1f / 50f;
        [SerializeField, Header("進行イベント")] private UnityEvent advanceEvent = new UnityEvent();
        // 文字送り時イベント関係
        [SerializeField, Header("文字送り時イベント")] private UnityEvent charEvent = new UnityEvent();
        public UnityEvent CharEvent => charEvent;
        [SerializeField, Header("文字送り時イベント発生間隔(文字)")] int charEventInterval = 0;
        private int charCount = 0;

        
        // メッセージ表示アニメーション関係
        string allMessage = "";
        bool isPlaying = false;
        int displayLength = 0;
        float playTimeCount = 0f;
        int oldDisplayLength = 0;

        void Update() {
            if(isPlaying) {
                playTimeCount += Time.deltaTime;
                if(timePerChar == 0) {
                    displayLength = allMessage.Length;
                } else {
                    displayLength = Mathf.Clamp((int)(playTimeCount / timePerChar), 0, allMessage.Length);
                }
                // 表示
                messageText.text = allMessage.Substring(0, displayLength);
                if(displayLength == allMessage.Length) isPlaying = false;
                // 文字数が進んだらカウント
                if(displayLength > oldDisplayLength) {
                    charCount += displayLength - oldDisplayLength;
                }
                // カウントがインターバルを超えたら文字表示イベントを実行
                if(charEventInterval != 0) {
                    if(charCount >= charEventInterval) {
                        charEvent.Invoke();
                        charCount = 0;
                    }
                }
                
                oldDisplayLength = displayLength;
            }
        }

        public void DisplayMessage(string speaker, string message) {
            Open();
            speakerText.text = speaker;
            messageText.text = "";
            allMessage = message;
            isPlaying = true;
            displayLength = 0;
            playTimeCount = 0f;
            oldDisplayLength = 0;
            if(charEventInterval == 0) {
                charCount = 0;
            } else {
                charCount = charEventInterval - 1;
            }
        }

        public void DisplayAll() {
            playTimeCount = timePerChar * allMessage.Length;
        }

        public void Advance() {
            if(isPlaying) {
                DisplayAll();
            } else {
                advanceEvent.Invoke();
            }
        }

        public void Open() {
            rootObject.SetActive(true);
        }
        public void Close() {
            rootObject.SetActive(false);
        }
    }
}