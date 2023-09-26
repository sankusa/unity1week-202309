using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace Sankusa.unity1week202309.InputManagement
{
    public interface IInputProvider
    {
        bool GetMainButtonDown();
        IObservable<Unit> OnMainButtonPush {get;}
    }
}