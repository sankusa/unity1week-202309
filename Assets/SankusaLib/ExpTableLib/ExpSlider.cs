using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Components;
using DG.Tweening;
using UnityEngine.Events;
using System;
using UniRx;

namespace SankusaLib.ExpTableLib {
    public class ExpSlider : MonoBehaviour
    {
        [SerializeField] private string levelTextPrefix;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private UISlider expSlider;
        [SerializeField] private TMP_Text expText;
        [SerializeField] private ExpTable expTable = null;
        [SerializeField] private UnityEvent onSetExp = new UnityEvent();
        [SerializeField] private UnityEvent<int> onLevelUp = new UnityEvent<int>();
        private DoubleReactiveProperty displayExp = new DoubleReactiveProperty(0);
        private long exp = 0;

        public int Level => expTable.ExpToLevel(exp);
        private int levelOld = 0;

        public long RemainderExp => expTable.ExpToRemainderExp(exp);

        private bool quietUpdate = false;

        void Start() {
            quietUpdate = true;
            this.displayExp.Subscribe(_ => OnDisplayExpChanged());
        }

        private void OnDisplayExpChanged() {
            if(expTable == null) return;

            int level = expTable.ExpToLevel((long)displayExp.Value);
            if(levelText != null) levelText.text = levelTextPrefix + level;
            if(expSlider != null) expSlider.value = (float)(displayExp.Value - expTable.LevelToTotalExp(expTable.ExpToLevel((long)displayExp.Value)))/ expTable.ExpToRequiredExp((long)displayExp.Value);
            if(expText != null) expText.text = expTable.ExpToRemainderExp((long)displayExp.Value).ToString() + "/" + expTable.ExpToRequiredExp((long)displayExp.Value).ToString();

            if(!quietUpdate) {
                if(level > levelOld) onLevelUp.Invoke(level);
            } else {
                quietUpdate = false;
            }
            
            levelOld = level;
        }

        public void SetExp(long exp) {
            this.exp = exp;
            DOTween.To(() => displayExp.Value, value => displayExp.Value = value, exp, 0.5f);
            onSetExp.Invoke();
        }

        public void SetExpQuiet(long exp) {
            this.exp = exp;
            displayExp.Value = exp;

            quietUpdate = true;
        }

        public void SetExpTable(ExpTable expTable) {
            this.expTable = expTable;
        }
    }
}