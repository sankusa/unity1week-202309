using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Components;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public class EnemyInfoView : MonoBehaviour
    {
        private EnemyCore _enemyCore;
        [SerializeField] private EnemyInfoMaster _enemyInfoMaster;
        [SerializeField] private TMP_Text _nametext;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private UISlider _hpBar;
        [SerializeField] private TMP_Text _hpText;

        public void SetEnemyCore(EnemyCore enemyCore)
        {
            _enemyCore = enemyCore;
            if(enemyCore != null) gameObject.SetActive(true);
        }

        void Update()
        {
            if(_enemyCore != null)
            {
                EnemyInfo info = _enemyInfoMaster.FindById(_enemyCore.EnemyId);
                _nametext.text = info.name;
                _descriptionText.text = info.Description;
                _hpBar.value = (float) _enemyCore.Hp.Value / _enemyCore.HpMax;
                _hpText.text = $"{_enemyCore.Hp.Value} / {_enemyCore.HpMax}";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}