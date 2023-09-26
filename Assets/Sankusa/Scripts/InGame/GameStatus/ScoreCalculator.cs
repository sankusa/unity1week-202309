using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Sankusa.unity1week202309.InGame.Player;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class ScoreCalculator : IDisposable
    {
        private readonly PlayerCharacterStatus _playerStatus;
        private readonly ScoreModel _scoreModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public ScoreCalculator(PlayerCharacterStatus playerStatus, ScoreModel scoreModel)
        {
            playerStatus.OnAddEnergy.Subscribe(x =>
            {
                scoreModel.Add(x);
            })
            .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}