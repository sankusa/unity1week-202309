using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterEnergyView : MonoBehaviour
    {
        [SerializeField] private UISlider _energySlider;
        [SerializeField] private TMP_Text _energyText;
        [Inject] private PlayerCharacterStatus _status;

        void Start()
        {
            Observable.Merge(
                this.ObserveEveryValueChanged(_ => _status.Energy),
                this.ObserveEveryValueChanged(_ => _status.RequiredEnergy)
            )
            .Subscribe(_ =>
            {
                _energySlider.value = (float) _status.Energy / _status.RequiredEnergy;
                _energyText.SetText("{0} / {1}", _status.Energy, _status.RequiredEnergy);
            })
            .AddTo(this);
        }
    }
}