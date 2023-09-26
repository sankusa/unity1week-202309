using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class ScoreModel : IDisposable
    {
        private readonly ReactiveProperty<int> _score = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> Score => _score;

        public void Add(int value)
        {
            _score.Value += value;
        }

        public void Reset()
        {
            _score.Value = 0;
        }

        public void Dispose()
        {
            _score.Dispose();
        }
    }
}