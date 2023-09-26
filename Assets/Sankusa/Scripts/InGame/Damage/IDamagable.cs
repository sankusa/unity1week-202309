using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Damage
{
    public interface IDamagable
    {
        void AddDamage(DamageData damageData);
    }
}