using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerAttackView : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _attackText;

        public void SetAttack(int attack)
        {
            _attackText.SetText("攻撃力 {0}", attack);
        }
    }
}