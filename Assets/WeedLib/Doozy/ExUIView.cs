using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine.Events;
using System;

namespace WeedLib.Doozy
{
    // UIViewの機能拡張版
    // UIViewは単に表示/非表示という状態を変数で持っているが、それとは別に概念的に「開いている」という状態を使ったイベント制御をするために作成。
    // また、バック遷移の概念も追加。
    public class ExUIView : MonoBehaviour
    {
        [SerializeField] private UIView _uiView;
        private bool _isOpened = false;
        public bool IsOpened => _isOpened;
        private Action _onClose;

        public void Open(Action onClose = null)
        {
            if(_isOpened)
            {
                Debug.Log($"{gameObject.name}'s ExUIView is already opened.");
                return;
            }

            _onClose = onClose;
            _isOpened = true;

            _uiView.Show();
            
        }

        public void Close()
        {
            if(!_isOpened)
            {
                Debug.Log($"{gameObject.name}'s ExUIView is not opened.");
                return;
            }

            _onClose?.Invoke();

            _onClose = null;
            _isOpened = false;

            _uiView.Hide();
        }

        public void Show()
        {
            _uiView.Show();
        }

        public void Hide()
        {
            _uiView.Hide();
        }
    }
}