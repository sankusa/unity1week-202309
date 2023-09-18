using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WeedLib.SimpleMessage
{
    public class SimpleMessageViewer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float showDuration = 0.5f;
        [SerializeField] private float fadeOutDuration = 0.5f;
        private float showTimer;
        private float fadeOutTimer;

        void Start()
        {
            SimpleMessageCore.OnSetMessage.AddListener(SetMessage);
        }

        void Update()
        {
            if(showTimer > 0)
            {
                showTimer -= Time.deltaTime;
            }
            else if(fadeOutTimer > 0)
            {
                fadeOutTimer -= Time.deltaTime;
                canvasGroup.alpha = (fadeOutDuration == 0 ? 0 : fadeOutTimer / fadeOutDuration);
            }
        }

        void OnDestroy()
        {
            SimpleMessageCore.OnSetMessage.RemoveListener(SetMessage);
        }

        private void SetMessage(string message)
        {
            canvasGroup.alpha = 1;
            messageText.text = message;
            showTimer = showDuration;
            fadeOutTimer = fadeOutDuration;
        }
    }
}