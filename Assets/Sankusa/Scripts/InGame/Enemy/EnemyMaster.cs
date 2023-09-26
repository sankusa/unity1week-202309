using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    [CreateAssetMenu(fileName = nameof(EnemyMaster), menuName = nameof(EnemyMaster))]
    public class EnemyMaster : ScriptableObject
    {
        [SerializeField] private List<EnemyCore> _enemyPrefabs;
        public EnemyCore GetByIndex(int index)
        {
            return _enemyPrefabs[index];
        }
    }
}