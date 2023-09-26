using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterStaminaView : MonoBehaviour
    {
        [SerializeField] private UISlider _staminaSlider;
        [SerializeField] private TMP_Text _staminaText;
        [Inject] private PlayerCharacterStatus _status;

        void Start()
        {
            Observable.Merge(
                this.ObserveEveryValueChanged(_ => _status.Stamina),
                this.ObserveEveryValueChanged(_ => _status.StaminaMax)
            )
            .Subscribe(_ =>
            {
                _staminaSlider.value = (float) _status.Stamina / _status.StaminaMax;
                _staminaText.SetText("{0:0}", _status.Stamina);
            })
            .AddTo(this);
        }
    }
}