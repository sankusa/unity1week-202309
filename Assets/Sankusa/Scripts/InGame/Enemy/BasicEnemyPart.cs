using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Sankusa.unity1week202309.InGame.Damage;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class BasicEnemyPart : MonoBehaviour , IEnemyPart, IDamagable
    {
        [SerializeField] private int _deffence;
        private Subject<int> _onReceiveDamageSubject = new Subject<int>();
        public IObservable<int> OnReceiveDamage => _onReceiveDamageSubject;

        public void AddDamage(DamageData damageData)
        {
            Debug.Log(damageData.Attack);
            _onReceiveDamageSubject.OnNext(Mathf.Max(damageData.Attack - _deffence, 0));
        }
    }
}