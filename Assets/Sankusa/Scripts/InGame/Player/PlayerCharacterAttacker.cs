using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sankusa.unity1week202309.InGame.Damage;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterAttacker : PlayerCharacterComponentBase
    {
        // 本当は接触した敵のEnemyCoreをMessagePipeで通知したかったけど時間がないので直接EnemyInfoViewに設定
        [SerializeField] private Enemy.EnemyInfoView _enemyInfoView;
        [SerializeField] private PlayerAttackView _attackView;
        private PlayerCharacterMover _mover;
        protected override void OnInitialize()
        {
            _mover = GetComponent<PlayerCharacterMover>();

            _mover.OnCrash
                .Subscribe(x =>
                {
                    IDamagable damagable = x.Item1.gameObject.GetComponent<IDamagable>();

                    if(damagable != null)
                    {
                        Debug.Log(_core.Status.Attack + "/" + x.Item2.magnitude);
                        damagable.AddDamage(new DamageData((int)(x.Item2.magnitude)));
                    }

                    Enemy.EnemyCore core = x.Item1.gameObject.GetComponentInParent<Enemy.EnemyCore>();
                    if(core != null)
                    {
                        _enemyInfoView.SetEnemyCore(core);
                    }
                })
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _attackView.SetAttack((int)_mover.Velocity.Value.magnitude);
                })
                .AddTo(this);
        }
    }
}