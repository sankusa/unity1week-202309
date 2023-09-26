using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterHpView : MonoBehaviour
    {
        [SerializeField] private UISlider _hpSlider;
        [SerializeField] private TMP_Text _hpText;
        [Inject] private PlayerCharacterStatus _status;

        void Start()
        {
            Observable.Merge(
                this.ObserveEveryValueChanged(_ => _status.Hp),
                this.ObserveEveryValueChanged(_ => _status.HpMax)
            )
            .Subscribe(_ =>
            {
                _hpSlider.value = (float) _status.Hp / _status.HpMax;
                _hpText.SetText("{0}", _status.Hp);
            })
            .AddTo(this);
        }
    }
}