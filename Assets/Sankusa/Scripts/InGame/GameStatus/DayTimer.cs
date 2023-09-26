using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class DayTimer : IDisposable
    {
        private readonly ReactiveProperty<float> _elapsed = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> Elapsed => _elapsed;

        private readonly ReactiveProperty<float> _limit = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<float> Limit => _limit;

        public float Progress => _elapsed.Value / _limit.Value;

        public bool IsTimeOver => _elapsed.Value == _limit.Value;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void SetLimit(float value)
        {
            _limit.Value = value;
        }

        public void Reset()
        {
            _elapsed.Value = 0;
        }

        public void Start()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposables);
        }

        private void Update()
        {
            _elapsed.Value = Mathf.Clamp(_elapsed.Value + Time.deltaTime, 0, _limit.Value);
        }

        public void Stop()
        {
            _disposables.Clear();
        }

        public void Dispose()
        {
            _elapsed.Dispose();
            _limit.Dispose();
            _disposables.Dispose();
        }
    }
}