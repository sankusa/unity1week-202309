using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Sankusa.unity1week202309.InGame.Player
{
    public abstract class PlayerCharacterComponentBase : MonoBehaviour
    {
        protected PlayerCharacterCore _core;
        void Start()
        {
            _core = GetComponent<PlayerCharacterCore>();

            _core.OnInitializeAsync
                .Subscribe(_ => OnInitialize())
                .AddTo(this);
        }

        protected virtual void OnInitialize() {}
    }
}