using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class Enemy1AI : EnemyComponentBase
    {
        [SerializeField] private float _downSpeed;
        [SerializeField] private float _hopSpeed;
        [SerializeField] private float _hopDuration;
        [SerializeField] private float _runawaySpeed;
        private float _hopAndDownSpeed;
        private Vector2 _runawayVelocity;
        private EnemyCharacterController _enemyCharacterController;

        protected override void OnInitialize()
        {
            _enemyCharacterController = GetComponent<EnemyCharacterController>();
            DOTween.To(() => _hopAndDownSpeed, value => _hopAndDownSpeed = value, _downSpeed, _hopDuration).From(_hopSpeed).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Restart).SetLink(gameObject);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    if((_playerCore.transform.position - transform.position).magnitude < 5)
                    {
                        _runawayVelocity = (transform.position - _playerCore.transform.position).normalized * _runawaySpeed;
                    }
                    else
                    {
                        _runawayVelocity = Vector2.zero;
                    }
                    Vector3 scale = transform.localScale;
                    if(_runawayVelocity.x > 0)
                    {
                        scale.x = -Mathf.Abs(scale.x);
                    }
                    else if(_runawayVelocity.x < 0)
                    {
                        scale.x = Mathf.Abs(scale.x);
                    }
                    transform.localScale = scale;
                    _enemyCharacterController.Velocity = _runawayVelocity + new Vector2(0, _hopAndDownSpeed);
                })
                .AddTo(this);
        }
    }
}