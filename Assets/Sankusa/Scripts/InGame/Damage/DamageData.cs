using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Damage
{
    public struct DamageData
    {
        private readonly int _attack;
        public int Attack => _attack;

        public DamageData(int attack)
        {
            _attack = attack;
        }
    }
}