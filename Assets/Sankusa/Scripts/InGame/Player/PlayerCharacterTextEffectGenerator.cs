using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SankusaLib;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterTextEffectGenerator : PlayerCharacterComponentBase
    {
        [SerializeField] private Transform _textEffectGeneratePosMarker;
        [SerializeField] private TextEffect _positiveTextEffectPrefab;
        [SerializeField] private TextEffect _damageTextEffectPrefab;

        protected override void OnInitialize()
        {
            _core.Status.OnAddEnergy
                .Subscribe(x =>
                {
                    Instantiate(_positiveTextEffectPrefab, _textEffectGeneratePosMarker.position, Quaternion.identity).Text = $"栄養 +{x.ToString()}";
                })
                .AddTo(this);

            _core.Status.OnRemoveHp
                .Subscribe(x =>
                {
                    Instantiate(_damageTextEffectPrefab, _textEffectGeneratePosMarker.position, Quaternion.identity, transform).Text = x.ToString();
                })
                .AddTo(this);
        }
    }
}