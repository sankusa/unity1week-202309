using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public interface IEnemyPart
    {
        IObservable<int> OnReceiveDamage { get; }
    }
}