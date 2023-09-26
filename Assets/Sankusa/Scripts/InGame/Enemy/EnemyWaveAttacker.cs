using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Sankusa.unity1week202309.InGame.Damage;
using WeedLib;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyWaveAttacker : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _attackMax;
        [SerializeField] private float _duration;
        private float _attack;

        void Start()
        {
            DOTween.To(() => _attack, value => _attack = value, _attackMax, _duration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);

            this.OnCollisionEnter2DAsObservable()
                .Subscribe(col =>
                {
                    IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
                    Debug.Log("EWA" + (damagable != null));
                    if(damagable != null)
                    {
                        damagable.AddDamage(new DamageData((int)_attack));
                    }
                })
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    _spriteRenderer.color = new Color(1, 1 - _attack / _attackMax, 1 - _attack / _attackMax);
                })
                .AddTo(this);
        }
    }
}