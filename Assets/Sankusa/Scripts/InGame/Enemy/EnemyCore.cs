using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Zenject;
using Sankusa.unity1week202309.InGame.Player;
using System.Linq;
using UnityEngine.Assertions;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    // 敵の最低構成要素
    public class EnemyCore : MonoBehaviour
    {
        [SerializeField] private string _enemyId;
        public string EnemyId => _enemyId;
        // IEnemyPartシリアライズ不能のため、MonoBehaviourでシリアライズし、初期化時にキャストする
        [SerializeField] private List<MonoBehaviour> _partsMonobehaviours;
        private List<IEnemyPart> _parts;

        private ReactiveProperty<int> _hp = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> Hp => _hp;

        [SerializeField] private int _hpMax;
        public int HpMax => _hpMax;

        public bool HpIsFull => _hp.Value == _hpMax;

        private Subject<int> _onRemoveHpSubject = new Subject<int>();
        public IObservable<int> OnRemoveHp => _onRemoveHpSubject;

        private Subject<Unit> _onDeadSubject = new Subject<Unit>();
        public IObservable<Unit> OnDead => _onDeadSubject;

        [Inject] private EnemyProvider _enemyProvider;
        
        void Awake()
        {
            // IEnemyPart以外が紛れてたらアウト
            Assert.IsTrue(_partsMonobehaviours.Select(x => x as IEnemyPart).Where(x => x == null).Count() == 0);

            _hp.Value = _hpMax;

            _hp.Subscribe(x =>
            {
                if(x == 0)
                {
                    Die();
                }
            })
            .AddTo(this);

            // IEnemyPartにキャスト
            _parts = _partsMonobehaviours.Select(x => x as IEnemyPart).ToList();
            
            foreach(IEnemyPart part in _parts)
            {
                part.OnReceiveDamage.Subscribe(damage =>
                {
                    RemoveHp(damage);
                })
                .AddTo(part as MonoBehaviour);
            }

            _enemyProvider.Add(this);
        }

        public void RemoveHp(int value)
        {
            _onRemoveHpSubject.OnNext(value);
            _hp.Value = Mathf.Max(_hp.Value - value, 0);
        }

        private void Die()
        {
            _onDeadSubject.OnNext(Unit.Default);
            _enemyProvider.Remove(this);
            Destroy(gameObject);
        }
    }
}