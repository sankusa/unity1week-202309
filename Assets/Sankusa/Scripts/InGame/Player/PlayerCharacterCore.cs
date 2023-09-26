using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    // パラメータの公開
    public class PlayerCharacterCore : MonoBehaviour
    {
        [Inject] private PlayerCharacterStatus _status;
        public PlayerCharacterStatus Status => _status;
        private readonly AsyncSubject<Unit> _onInitializeAsyncSubject = new AsyncSubject<Unit>();
        public IObservable<Unit> OnInitializeAsync => _onInitializeAsyncSubject;
        [Inject] private PlayerCharacterProvider _playerProvider;

        void Start()
        {
            _onInitializeAsyncSubject.OnNext(Unit.Default);
            _onInitializeAsyncSubject.OnCompleted();

            _playerProvider.Set(this);
        }

        void Update()
        {
            _status.Update(Time.deltaTime);
        }

        void OnDestroy()
        {
            _onInitializeAsyncSubject.Dispose();
        }
    }
}