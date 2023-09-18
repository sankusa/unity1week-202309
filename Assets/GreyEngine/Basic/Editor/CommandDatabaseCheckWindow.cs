using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreyEngine.Basic.TypeConversion;

namespace GreyEngine.Basic {
    public class CommandDatabaseCheckWindow : EditorWindow
    {
        private CommandDatabase database = new CommandDatabase();
        //private MasterTypeConverter masterConverter = new MasterTypeConverter();
        Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Tools/GreyEngine/Display/CommandDatabase")]
        public static void ShowWindow() {
            EditorWindow.GetWindow<CommandDatabaseCheckWindow>("CommandDatabase");
        }

        void OnEnable() {
            database = CommandDatabase.LoadDatabase();
        }

        void OnGUI() {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            using(new EditorGUILayout.VerticalScope()) {
                if(database.datas.Count == 0) EditorGUILayout.LabelField("Nothing");
                foreach(CommandData data in database.datas) {
                    using(new EditorGUILayout.HorizontalScope()) {
                        EditorGUILayout.TextField(data.category.ToString());
                        EditorGUILayout.TextField(data.className);
                        EditorGUILayout.TextField(CommandData.CreateMethodLabel(data.methodName, data.argTypeNames, data.argNames));
                        EditorGUILayout.TextField(data.returnTypeName);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}