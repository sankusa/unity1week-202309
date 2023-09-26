using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Cysharp.Threading.Tasks;
using System.Threading;
using SankusaLib.SoundLib;

namespace Sankusa.unity1week202309.InGame.Performer
{
    public class DeadPerformer : MonoBehaviour
    {
        [SerializeField] private UIView _uiView;
        [SerializeField, SoundId] private string _gameoverSeId;

        public async UniTask Perform(CancellationToken token)
        {
            _uiView.Show();

            SoundManager.Instance.PlaySe(_gameoverSeId);

            await UniTask.WaitUntil(() => _uiView.isVisible, cancellationToken: token);
        }
    }
}
