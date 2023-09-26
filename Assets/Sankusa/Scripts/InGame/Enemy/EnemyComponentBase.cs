using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sankusa.unity1week202309.InGame.Player;
using Zenject;

namespace Sankusa.unity1week202309.InGame.Enemy
{
    public abstract class EnemyComponentBase : MonoBehaviour
    {
        protected EnemyCore _core;
        [Inject] private PlayerCharacterProvider _playerProvider;
        protected PlayerCharacterCore _playerCore;
        void Start()
        {
            _core = GetComponent<EnemyCore>();

            StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            yield return new WaitUntil(() => _playerProvider.Player != null);
            _playerCore = _playerProvider.Player;

            OnInitialize();
        }

        protected virtual void OnInitialize() {}
    }
}