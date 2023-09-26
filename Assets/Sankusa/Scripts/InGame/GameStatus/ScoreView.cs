using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private float _ScoreFadeDuration;
        private ReactiveProperty<int> _scoreForDisplay = new ReactiveProperty<int>();
        private Tweener _scoreForDisplayFadeTweener;

        void Start()
        {
            _scoreForDisplay.Subscribe(x =>
            {
                _scoreText.SetText("{0}", x);
            })
            .AddTo(this);
        }

        public void SetScore(int score)
        {
            if(_scoreForDisplayFadeTweener != null && _scoreForDisplayFadeTweener.IsActive() && _scoreForDisplayFadeTweener.IsPlaying())
            {
                _scoreForDisplayFadeTweener.Kill();
                _scoreForDisplayFadeTweener = null;
            }

            _scoreForDisplayFadeTweener = DOTween.To(() => _scoreForDisplay.Value, value => _scoreForDisplay.Value = value, score, _ScoreFadeDuration).SetLink(gameObject);
        }
    }
}