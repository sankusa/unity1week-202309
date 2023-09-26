using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.Player
{
    public class PlayerCharacterProvider
    {
        private PlayerCharacterCore _player;
        public PlayerCharacterCore Player => _player;

        private readonly AsyncSubject<PlayerCharacterCore> _onPlayerSetAsyncSubject = new AsyncSubject<PlayerCharacterCore>();
        public IObservable<PlayerCharacterCore> OnPlayerSetAsyncSubject => _onPlayerSetAsyncSubject;

        public void Set(PlayerCharacterCore player)
        {
            _player = player;

            _onPlayerSetAsyncSubject.OnNext(player);
            _onPlayerSetAsyncSubject.OnCompleted();
        }
    }
}