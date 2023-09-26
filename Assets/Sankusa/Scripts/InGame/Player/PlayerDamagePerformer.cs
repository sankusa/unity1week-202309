using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using WeedLib;
using SankusaLib.SoundLib;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerDamagePerformer : MonoBehaviour
    {
        [Inject] private PlayerCharacterStatus _playerStatus;
        [SerializeField] private Image _damageImage;
        [SerializeField] private float _fadeDuration;
        [SerializeField, SoundId] private string _damageSeId;
        private Tweener _fadeTweener;

        void Start()
        {
            _playerStatus.OnRemoveHp.Subscribe(x =>
            {
                _fadeTweener.SafeKill();
                _fadeTweener = _damageImage.DOFade(0, _fadeDuration).From(1).SetLink(gameObject);
                SoundManager.Instance.PlaySe(_damageSeId);
            })
            .AddTo(this);
        }
    }
}