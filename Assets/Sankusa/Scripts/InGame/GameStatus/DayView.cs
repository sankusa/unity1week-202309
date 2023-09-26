using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using TMPro;

namespace Sankusa.unity1week202309.InGame.GameStatus
{
    public class DayView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _dayText;
        [Inject] private DayModel _dayModel;
        void Start()
        {
            _dayModel.Day.Subscribe(x =>
            {
                _dayText.SetText("{0}", x);
            })
            .AddTo(this);
        }
    }
}