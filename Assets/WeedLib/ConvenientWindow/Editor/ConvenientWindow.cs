using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeedLib.ConvenientWindow
{
    public class ConvenientWindow : EditorWindow
    {
        [MenuItem("WeedLib/" + nameof(ConvenientWindow))]
        private static void Open() {
            GetWindow<ConvenientWindow>();
        }

        void OnGUI()
        {
            Time.timeScale = Mathf.Max(EditorGUILayout.FloatField("TimeScale", Time.timeScale), 0);

            using(new EditorGUILayout.HorizontalScope())
            {
                if(GUILayout.Button("自動コンパイル ON"))
                {
                    EditorApplication.UnlockReloadAssemblies();
                    Debug.Log("自動コンパイルをONにしました。");
                }
                if(GUILayout.Button("自動コンパイル OFF"))
                {
                    EditorApplication.LockReloadAssemblies();
                    Debug.Log("自動コンパイルをOFFにしました。手動コンパイルは[Ctr+R]。");
                }
            }
        }
    }
}