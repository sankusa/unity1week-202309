using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Zenject;

namespace Sankusa.unity1week202309.InGame.Player
{
    // 可読性UPのため、ReactivePropertyを無しにしてみた
    public class PlayerCharacterStatus : IDisposable
    {
        private int _hp;
        public int Hp
        {
            get => _hp;
            private set => _hp = Mathf.Clamp(value, 0, HpMax);
        }

        private int _hpMax;
        public int HpMax => _hpMax;

        private float _stamina;
        public float Stamina
        {
            get => _stamina;
            private set => _stamina = Mathf.Clamp(value, 0, _staminaMax);
        }

        private float _staminaMax;
        public float StaminaMax => _staminaMax;

        private float _staminaRecoverSpeedBase;
        public float StaminaRecoverSpeed => _staminaRecoverSpeedBase;

        private float _driveCostStamina;
        public float DriveCostStamina => _driveCostStamina;

        private int _attackBase;
        public int Attack => _attackBase;

        private int _deffence;
        public int Deffence => _deffence;

        private int _energy;
        public int Energy
        {
            get => _energy;
            set => _energy = Mathf.Clamp(value, 0, _requierdEnergy);
        }

        private int _requierdEnergy;
        public int RequiredEnergy => _requierdEnergy;

        public bool EnergyIsFull => _energy >= _requierdEnergy;

        private float _driveSpeed;
        public float DriveSpeed => _driveSpeed;

        private readonly Subject<int> _onRemoveHpSubject = new Subject<int>();
        public IObservable<int> OnRemoveHp => _onRemoveHpSubject;

        private readonly Subject<int> _onAddEnergySubject = new Subject<int>();
        public IObservable<int> OnAddEnergy => _onAddEnergySubject;

        private bool _recoverStamina = true;
        public bool RecoverStamina
        {
            get => _recoverStamina;
            set => _recoverStamina = value;
        }

        [Inject]
        public PlayerCharacterStatus(PlayerCharacterDefaultStatusPreset preset)
        {
            _hp = preset.DefaultHp;
            _hpMax = preset.DefaultHp;
            _stamina = preset.DefaultStamina;
            _staminaMax = preset.DefaultStamina;
            _staminaRecoverSpeedBase = preset.DefaultStaminaRecoverSpeed;
            _driveCostStamina = preset.DefaultDriveCostStamina;
            _attackBase = preset.DefaultAttack;
            _deffence = preset.DefaultDeffence;
            _energy = 0;
            _requierdEnergy = preset.DefaultRequiredEnergy;
            _driveSpeed = preset.DefaultDriveSpeed;
        }

        // 日またぎ処理
        public void Reflesh()
        {
            _hp = _hpMax;
            _stamina = _staminaMax;
            _energy = 0;
        }

        public void RemoveHp(int value)
        {
            Hp -= value;
            _onRemoveHpSubject.OnNext(value);
        }

        public void AddStamina(float value)
        {
            Stamina += value;
        }

        public bool TryRemoveStamina(float value)
        {
            if(_stamina < value) return false;

            _stamina -= value;
            return true;
        }

        public void AddEnergy(int value)
        {
            Energy += value;
            _onAddEnergySubject.OnNext(value);
        }

        public void Update(float deltaTime)
        {
            if(_recoverStamina)
            {
                AddStamina(StaminaRecoverSpeed * deltaTime);
            }
        }

        public void Dispose()
        {
            _onAddEnergySubject.Dispose();
        }
    }
}