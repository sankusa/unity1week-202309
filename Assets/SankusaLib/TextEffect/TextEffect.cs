using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

namespace SankusaLib {
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private TextMeshProUGUI textPro;
        public string Text {
            set {
                if(this.text != null) this.text.text = value;
                if(textPro != null) textPro.text = value;
            }
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration;

        [SerializeField] private float toAlpha;
        [SerializeField] private Ease alphaEase;

        [SerializeField] private Vector3 moveOffset;
        [SerializeField] private Ease moveEase;

        [SerializeField] private UnityEvent onStart = new UnityEvent();
        public UnityEvent OnStart => onStart;
        [SerializeField] private UnityEvent onEnd = new UnityEvent();
        public UnityEvent OnEnd => onEnd;

        void Start() {
            onStart.Invoke();
            transform.DOMove(transform.position + moveOffset, duration)
                    .SetEase(moveEase).OnComplete(() => {
                        onEnd.Invoke();
                        Destroy(gameObject);
                    }).SetLink(gameObject);
            canvasGroup.DOFade(toAlpha, duration).SetEase(alphaEase).SetLink(gameObject);
        }
    }
}