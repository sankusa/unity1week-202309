using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Sankusa.unity1week202309.InputManagement;
using System;
using SankusaLib.SoundLib;

namespace Sankusa.unity1week202309.InGame.Player
{
    [RequireComponent(typeof(PlayerCharacterController))]
    [RequireComponent(typeof(PlayerCharacterRotaryPointer))]
    public class PlayerCharacterMover : PlayerCharacterComponentBase
    {
        [SerializeField] private float _repulsion;
        [SerializeField] private float _attenuation;
        [SerializeField] private float _rotaryPointerStopDurationAfterDrive;
        [SerializeField, SoundId] private string _crashSeId;
        private PlayerCharacterController _playerCharacterController;
        private PlayerCharacterRotaryPointer _playerCharacerRotaryPointer;
        [Inject] private IInputProvider _inputProvider;

        private readonly ReactiveProperty<Vector2> _velocity = new ReactiveProperty<Vector2>();
        public IReadOnlyReactiveProperty<Vector2> Velocity => _velocity;

        private float _rotaryPointerStopTimer;

        private readonly Subject<Vector2> _onDriveSubject = new Subject<Vector2>();
        public IObservable<Vector2> OnDrive => _onDriveSubject;

        // CollisionEnter2Dに弾かれる前の速度を加えて通知する
        private readonly Subject<(Collision2D, Vector2)> _onCrashSubject = new Subject<(Collision2D, Vector2)>();
        public IObservable<(Collision2D, Vector2)> OnCrash => _onCrashSubject;

        protected override void OnInitialize()
        {
            _playerCharacterController = GetComponent<PlayerCharacterController>();
            _playerCharacerRotaryPointer = GetComponent<PlayerCharacterRotaryPointer>();

            // 推進処理
            _inputProvider.OnMainButtonPush
                .Subscribe(_ =>
                {
                    if(_core.Status.TryRemoveStamina(_core.Status.DriveCostStamina))
                    {
                        _velocity.Value += _playerCharacerRotaryPointer.RotaryPointer.Direction * _core.Status.DriveSpeed;
                        _rotaryPointerStopTimer = _rotaryPointerStopDurationAfterDrive;
                        _onDriveSubject.OnNext(_playerCharacerRotaryPointer.RotaryPointer.Direction);
                    }
                })
                .AddTo(this);

            // 衝突時
            this.OnCollisionEnter2DAsObservable()
                .Subscribe(col =>
                {
                    Debug.Log("contactCount : " + col.contactCount);
                    for(int i = 0; i < col.contactCount; i++)
                    {
                        Debug.Log(col.contacts[i].point);
                    }
                    // 詳しい挙動は不明だが敵撃破時にcontactCountが0の場合が発生したのでcontactCountをチェック
                    if(col.contactCount > 0)
                    {
                        Vector2 contactPos = col.contacts[0].point;
                        Vector2 counterPosLocal = contactPos - (Vector2)transform.position;
                        // 接触点と逆方向に弾かれる
                        Vector2 velocityOld = _velocity.Value;
                        _velocity.Value = -counterPosLocal.normalized * _repulsion;
                        SoundManager.Instance.PlaySe(_crashSeId);

                        _onCrashSubject.OnNext((col, velocityOld));
                    }
                })
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    // タイマー消費
                    if(_rotaryPointerStopTimer > 0)
                    {
                        _rotaryPointerStopTimer = Mathf.Max(_rotaryPointerStopTimer - Time.deltaTime, 0);
                    }

                    // 回転ポインタの状態更新
                    _playerCharacerRotaryPointer.RotaryPointer.Active = (_rotaryPointerStopTimer == 0);
                    // スタミナ回復状態更新
                    _core.Status.RecoverStamina = (_rotaryPointerStopTimer == 0);
                })
                .AddTo(this);

            Observable.EveryFixedUpdate()
                .Subscribe(_ =>
                {
                    // 速度減衰
                    //_velocity.Value = _velocity.Value.normalized * (_velocity.Value.magnitude - _core.Status.Attenuation * Time.fixedDeltaTime);
                    _velocity.Value -= _velocity.Value * _attenuation;
                })
                .AddTo(this);

            _velocity.Subscribe(x =>
            {
                _playerCharacterController.Velocity = x;
            })
            .AddTo(this);
        }
    }
}