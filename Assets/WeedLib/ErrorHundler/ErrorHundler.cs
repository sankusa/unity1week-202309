using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeedLib
{
    public class ErrorHundler : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onReceiveException;

        void OnEnable()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        void OnDisabled()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string logString, string stackTrace, LogType logType)
        {
            if(logType == LogType.Exception)
            {
                _onReceiveException.Invoke();
            }
        }
    }
}