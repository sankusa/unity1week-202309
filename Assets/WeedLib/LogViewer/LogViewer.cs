using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;

namespace WeedLib.LogViewer
{
    public class LogViewer : MonoBehaviour
    {
        private static List<(string log, string stackTrace, LogType logType)> logContainers = new List<(string log, string stackTrace, LogType logType)>();
        private static int logCapacity = 100;
        private static UnityEvent onLogMessageReceived = new UnityEvent();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }
        private static void OnLogMessageReceived(string log, string stackTrace, LogType logType)
        {
            AddLog(log, stackTrace, logType);
            onLogMessageReceived.Invoke();
        }
        private static void AddLog(string log, string stackTrace, LogType logType)
        {
            logContainers.Add((log, stackTrace, logType));
            if(logContainers.Count > logCapacity) logContainers.RemoveAt(0);
        }

        [SerializeField] private Text logText;
        [SerializeField] private bool showLog;
        [SerializeField] private bool showWarning;
        [SerializeField] private bool showError;
        [SerializeField] private bool showAssert;
        [SerializeField] private bool showException;
        [SerializeField, Min(1)] private int visibleNum = 1;
        [SerializeField] private int logLengthMax = 100;
        [SerializeField] private int stackTraceLengthMax = 100;

        // UpdateText内でのエラー時、無限にログが出力されないように1フレーム1更新の制限をかける
        private bool shouldUpdateText = false;

        void Awake()
        {
            onLogMessageReceived.AddListener(ActivateShouldUpdateText);
        }

        void OnDestroy()
        {
            onLogMessageReceived.RemoveListener(ActivateShouldUpdateText);
        }

        void Start()
        {
            UpdateText();
        }

        void Update()
        {
            if(shouldUpdateText) UpdateText();
        }

        private void ActivateShouldUpdateText()
        {
            shouldUpdateText = true;
        }

        private void UpdateText()
        {
            int visibleLogCount = 0;
            string combinedLog = "";
            foreach(var logContainer in Enumerable.Reverse(logContainers).ToList())
            {
                if((logContainer.logType == LogType.Log && showLog)
                    || (logContainer.logType == LogType.Warning && showWarning)
                    || (logContainer.logType == LogType.Error && showError)
                    || (logContainer.logType == LogType.Assert && showAssert)
                    || (logContainer.logType == LogType.Exception && showException)
                )
                {
                    string currentLog = "";
                    if(logContainer.log.Length > logLengthMax)
                    {
                        currentLog += logContainer.log.Substring(0, logLengthMax) + "…";
                    }
                    else
                    {
                        currentLog += logContainer.log;
                    }
                    currentLog += "\n";
                    if(logContainer.stackTrace.Length > stackTraceLengthMax)
                    {
                        currentLog += logContainer.stackTrace.Substring(0, stackTraceLengthMax) + "…";
                    }
                    else
                    {
                        currentLog += logContainer.stackTrace;
                    }
                    currentLog += "\n";
                    currentLog += "\n";

                    if(logContainer.logType == LogType.Warning)
                    {
                        currentLog = $"<color=yellow>{currentLog}</color>";
                    }
                    else if(logContainer.logType == LogType.Error || logContainer.logType == LogType.Assert || logContainer.logType == LogType.Exception)
                    {
                        currentLog = $"<color=red>{currentLog}</color>";
                    }
                    combinedLog = currentLog + combinedLog;

                    // 表示数チェック
                    visibleLogCount++;
                    if(visibleLogCount >= visibleNum) break;
                }
            }
            logText.text = combinedLog;
        }
    }
}