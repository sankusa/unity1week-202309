using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using SankusaLib.SoundLib;

namespace Sankusa.unity1week202309.InGame.Player
{
    [RequireComponent(typeof(PlayerCharacterMover))]
    public class PlayerCharacterAnimator : PlayerCharacterComponentBase
    {
        [SerializeField, SoundId] private string _driveSeId;
        private static readonly string _triggerKeyDrive = "Drive";
        private static int _triggerHashDrive = Animator.StringToHash(_triggerKeyDrive);

        [SerializeField] private Animator _animator;
        [SerializeField] Transform _bodyRoot;
        private PlayerCharacterMover _mover;
        protected override void OnInitialize()
        {
            _mover = GetComponent<PlayerCharacterMover>();

            _mover.OnDrive.Subscribe(direction =>
            {
                _bodyRoot.rotation = Quaternion.FromToRotation(transform.up, direction);
                _animator.SetTrigger(_triggerHashDrive);
                SoundManager.Instance.PlaySe(_driveSeId);
            })
            .AddTo(this);
        }
    }
}