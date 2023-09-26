using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sankusa.unity1week202309.InGame.Damage;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private float _attack;

        void Start()
        {
            this.OnCollisionEnter2DAsObservable()
                .Subscribe(col =>
                {
                    IDamagable damagable = col.gameObject.GetComponent<IDamagable>();
                    Debug.Log(col.gameObject.name + "/" + (damagable != null));
                    if(damagable != null)
                    {
                        damagable.AddDamage(new DamageData((int)_attack));
                    }
                })
                .AddTo(this);
        }
    }
}