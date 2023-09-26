using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyProvider
    {
        private HashSet<EnemyCore> _enemies = new HashSet<EnemyCore>();
        public IEnumerable<EnemyCore> Enemies => _enemies;

        public void Add(EnemyCore enemy)
        {
            _enemies.Add(enemy);
        }

        public void Remove(EnemyCore enemy)
        {
            _enemies.Remove(enemy);
        }
    }
}