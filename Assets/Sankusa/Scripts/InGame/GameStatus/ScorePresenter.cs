using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class ScorePresenter : MonoBehaviour
    {
        [Inject] private ScoreModel _scoreModel;
        [SerializeField] private ScoreView _scoreView;

        void Start()
        {
            _scoreModel.Score.Subscribe(x =>
            {
                _scoreView.SetScore(x);
            })
            .AddTo(this);
        }
    }
}