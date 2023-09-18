using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

namespace SankusaLib {
    public class TextEffect2 : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPro;
        public string Text {
            set {
                if(textPro != null) textPro.text = value;
            }
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration1;
        [SerializeField] private float duration2;
        [SerializeField] private float duration3;

        [SerializeField] private Vector3 moveOffset;

        [SerializeField] private UnityEvent onStart = new UnityEvent();
        public UnityEvent OnStart => onStart;
        [SerializeField] private UnityEvent onEnd = new UnityEvent();
        public UnityEvent OnEnd => onEnd;

        private Vector3 defaultScale;

        void Start() {
            defaultScale = transform.localScale;
            transform.localScale = Vector3.zero;

            onStart.Invoke();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(defaultScale, duration1).SetEase(Ease.OutBack));
            sequence.Append(transform.DOScale(defaultScale, duration2).SetEase(Ease.Linear));
            sequence.Append(transform.DOMove(transform.position + moveOffset, duration3).SetEase(Ease.Linear));
            sequence.Join(canvasGroup.DOFade(0, duration3).SetEase(Ease.Linear));
            sequence.SetLink(gameObject);
            sequence.OnComplete(() => {
                onEnd.Invoke();
                Destroy(gameObject);
            });
        }

        public void SetText(string text)
        {
            textPro.text = text;
        }
    }
}