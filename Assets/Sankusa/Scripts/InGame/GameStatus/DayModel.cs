using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class DayModel : IDisposable
    {
        private readonly ReactiveProperty<int> _day = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> Day => _day;

        public void Increment()
        {
            _day.Value++;
        }

        public void Dispose()
        {
            _day.Dispose();
        }
    }
}