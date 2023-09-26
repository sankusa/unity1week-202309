using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using Zenject;
using UniRx;
using TMPro;
using System;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class DayTimerView : MonoBehaviour
    {
        [SerializeField] private UISlider _timeBar;
        [SerializeField] private TMP_Text _timeText;
        [Inject] private DayTimer _dayTimer;

        void Start()
        {
            Observable.Merge(_dayTimer.Elapsed.AsUnitObservable(), _dayTimer.Limit.AsUnitObservable())
                .Subscribe(_ =>
                {
                    _timeBar.value = _dayTimer.Progress;
                    _timeText.text = $"{TimeSpan.FromSeconds((double)(_dayTimer.Limit.Value - _dayTimer.Elapsed.Value)).ToString(@"mm\:ss")}";
                })
                .AddTo(this);
        }
    }
}