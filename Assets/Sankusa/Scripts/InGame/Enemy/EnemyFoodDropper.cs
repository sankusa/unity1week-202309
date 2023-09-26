using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sankusa.unity1week202309.InGame.Food;
using UniRx;
using Zenject;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyFoodDropper : EnemyComponentBase
    {
        [SerializeField] private Transform _foodDropPosMarker;
        [SerializeField] private FoodBase _foodPrefab;
        [Inject] private DiContainer _diContainer;

        protected override void OnInitialize()
        {
            _core.OnDead
                .Subscribe(_ =>
                {
                    _diContainer.InstantiatePrefab(_foodPrefab.gameObject, _foodDropPosMarker.position, Quaternion.identity, null);
                })
                .AddTo(this);
        }
    }
}