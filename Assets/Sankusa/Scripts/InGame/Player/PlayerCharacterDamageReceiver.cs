using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sankusa.unity1week202309.InGame.Damage;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterDamageReceiver : PlayerCharacterComponentBase, IDamagable
    {
        public void AddDamage(DamageData damageData)
        {
            _core.Status.RemoveHp(damageData.Attack - _core.Status.Deffence);
        }
    }
}