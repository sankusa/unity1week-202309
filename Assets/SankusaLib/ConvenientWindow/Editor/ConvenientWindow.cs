using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SankusaLib
{
    public class ConvenientWindow : EditorWindow
    {
        [MenuItem("SankusaLib/" + nameof(ConvenientWindow))]
        private static void Open()
        {
            GetWindow<ConvenientWindow>();
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }


        void OnGUI() {
            Time.timeScale = Mathf.Max(0, EditorGUILayout.FloatField("TimeScale", Time.timeScale));
        }
    }
}