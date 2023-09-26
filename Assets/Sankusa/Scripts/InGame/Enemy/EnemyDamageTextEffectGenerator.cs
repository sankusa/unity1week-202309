using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SankusaLib;
using Zenject;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyDamageTextEffectGenerator : EnemyComponentBase
    {
        [SerializeField] private Transform generatePosMarker;
        [SerializeField] private TextEffect _textEffectPrefab;

        protected override void OnInitialize()
        {
            _core.OnRemoveHp
                .Subscribe(x =>
                {
                    Instantiate(_textEffectPrefab, generatePosMarker.position, Quaternion.identity).GetComponent<TextEffect>().Text = x.ToString();
                })
                .AddTo(this);
        }
    }
}