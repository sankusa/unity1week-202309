using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InputManagement
{
    public class KeyboardInputProvider : IInputProvider, ITickable
    {
        private Subject<Unit> _onMainButtonPushSubject = new Subject<Unit>();
        public IObservable<Unit> OnMainButtonPush => _onMainButtonPushSubject;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public void Tick()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                _onMainButtonPushSubject.OnNext(Unit.Default);
            }
        }

        public bool GetMainButtonDown()
        {
            return Input.GetKeyDown(KeyCode.Alpha1);
        }
    }
}