using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.ExpTableLib {
    [CreateAssetMenu(fileName = nameof(ExpTable), menuName = nameof(SankusaLib) + "/" + nameof(ExpTableLib) + "/" + nameof(ExpTable))]
    public class ExpTable : ScriptableObject
    {
        [SerializeField] private int defaultLevel = 1;
        [SerializeField] private List<long> requiredExpList;

        public int MaxLevel {
            get {
                return requiredExpList.Count + defaultLevel;
            }
        }

        public long LevelToRequiredExp(int level) {
            int index = level - defaultLevel;
            return index < requiredExpList.Count ? requiredExpList[index] : 0;
        }
        
        public long LevelToTotalExp(int level) {
            long total = 0;
            for(int i = 0; i < Mathf.Min(level - defaultLevel, requiredExpList.Count - 1); i++) {
                total += requiredExpList[i];
            }
            return total;
        }

        public int ExpToLevel(long exp) {
            long sum = 0;
            for(int i = 0; i < requiredExpList.Count; i++) {
                sum += requiredExpList[i];
                if(exp < sum) return i + defaultLevel;
            }
            return requiredExpList.Count + defaultLevel;
        }

        public long ExpToRequiredExp(long exp) {
            return LevelToRequiredExp(ExpToLevel(exp));
        }

        public long ExpToRemainderExp(long exp) {
            return exp - LevelToTotalExp(ExpToLevel(exp));
        }
    }
}