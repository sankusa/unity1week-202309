using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    [Serializable]
    public class EnemyInfo
    {
        [SerializeField] private string _enemyId;
        public string EnemyId => _enemyId;
        [SerializeField] private string _name;
        public string name => _name;
        [SerializeField, TextArea] private string _description;
        public string Description => _description;
    }
    [CreateAssetMenu(fileName = nameof(EnemyInfoMaster), menuName = nameof(EnemyInfoMaster))]
    public class EnemyInfoMaster : ScriptableObject
    {
        [SerializeField] private List<EnemyInfo> _infoList;
        public EnemyInfo FindById(string enemyId)
        {
            return _infoList.Find(x => x.EnemyId == enemyId);
        }
    }
}