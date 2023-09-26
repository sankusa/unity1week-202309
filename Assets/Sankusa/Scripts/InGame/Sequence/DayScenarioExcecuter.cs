using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Zenject;
using Sankusa.unity1week202309.InGame.Enemy;
using Sankusa.unity1week202309.InGame.Stages;

namespace Sankusa.unity1week202309.InGame.Sequence
{
    public class DayScenarioExcecuter
    {
        private readonly EnemyMaster _enemyMaster;
        private readonly Stage _stage;
        private DiContainer _diContainer;

        [Inject]
        public DayScenarioExcecuter(EnemyMaster enemyMaster, Stage stage, DiContainer diContainer)
        {
            _enemyMaster = enemyMaster;
            _stage = stage;
            _diContainer = diContainer;
        }

        public async UniTask Execute(int day, CancellationToken token)
        {
            for(int i = 0; i < 25; i++)
            {
                EnemyCore enemyPrefab = _enemyMaster.GetByIndex(0);
                float x = 0;
                float y = 0;
                while(Mathf.Abs(x) < 5 && Mathf.Abs(y) < 5)
                {
                    x = _stage.ValidArea.min.x + 1 + Random.value * (_stage.ValidArea.max.x - _stage.ValidArea.min.x - 1);
                    y = _stage.ValidArea.min.y + 1 + Random.value * (_stage.ValidArea.max.y - _stage.ValidArea.min.y - 1);
                }
                _diContainer.InstantiatePrefab(enemyPrefab.gameObject, new Vector2(x, y), Quaternion.identity, null);
            }

            for(int i = 0; i < 3; i++)
            {
                EnemyCore enemyPrefab = _enemyMaster.GetByIndex(1);
                float x = 0;
                float y = 0;
                while(Mathf.Abs(x) < 5 && Mathf.Abs(y) < 5)
                {
                    x = _stage.ValidArea.min.x + 1 + Random.value * (_stage.ValidArea.max.x - _stage.ValidArea.min.x - 1);
                    y = _stage.ValidArea.min.y + 1 + Random.value * (_stage.ValidArea.max.y - _stage.ValidArea.min.y - 1);
                }
                _diContainer.InstantiatePrefab(enemyPrefab.gameObject, new Vector2(x, y), Quaternion.identity, null);
            }

            await UniTask.CompletedTask;
        }
    }
}