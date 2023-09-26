using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sankusa.unity1week202309.InGame.Player
{
    [CreateAssetMenu(menuName = nameof(PlayerCharacterDefaultStatusPreset), fileName = nameof(PlayerCharacterDefaultStatusPreset))]
    public class PlayerCharacterDefaultStatusPreset : ScriptableObject
    {
        [SerializeField] private int _defaultHp;
        [SerializeField] private int _defaultStamina;
        [SerializeField] private int _defaultStaminaRecoverSpeed;
        [SerializeField] private int _defaultDriveCostStamina;
        [SerializeField] private int _defaultAttack;
        [SerializeField] private int _defaultDeffence;
        [SerializeField] private int _defaultRequiredEnergy;
        [SerializeField] private int _defaultDriveSpeed;

        public int DefaultHp => _defaultHp;
        public int DefaultStamina => _defaultStamina;
        public int DefaultStaminaRecoverSpeed => _defaultStaminaRecoverSpeed;
        public int DefaultDriveCostStamina => _defaultDriveCostStamina;
        public int DefaultAttack => _defaultAttack;
        public int DefaultDeffence => _defaultDeffence;
        public int DefaultRequiredEnergy => _defaultRequiredEnergy;
        public int DefaultDriveSpeed => _defaultDriveSpeed;
    }
}