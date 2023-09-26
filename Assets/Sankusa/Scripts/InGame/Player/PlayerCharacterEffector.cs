using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    [RequireComponent(typeof(PlayerCharacterMover))]
    public class PlayerCharacterEffector : PlayerCharacterComponentBase
    {
        [SerializeField] private ParticleSystem _bubbleParticle;
        [SerializeField] private float _bubbleGenerateStartSpeed;
        private PlayerCharacterMover _mover;

        protected override void OnInitialize()
        {
            _mover = GetComponent<PlayerCharacterMover>();

            _mover.Velocity.Subscribe(velocity =>
            {
                if(velocity.magnitude > _bubbleGenerateStartSpeed)
                {
                    _bubbleParticle.Play();
                }
                else
                {
                    _bubbleParticle.Stop();
                }
            })
            .AddTo(this);
        }
    }
}