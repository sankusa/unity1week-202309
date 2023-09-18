using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GreyEngine.Basic {
    // コマンドを登録しておくScriptableObject
    [CreateAssetMenu(menuName = "GreyEngine/Create CommandTable")]
    public class CommandTable : ScriptableObject
    {
        public List<CommandData> datas = new List<CommandData>();
        public bool hideInRegister = false;
        
        // 存在チェック
        public bool Exists(CommandData data) {
            foreach(CommandData c in datas) {
                if(data.category == c.category && data.className == c.className && data.GetMethodKey() == c.GetMethodKey())  return true;
            }
            return false;
        }

        void Awake() {
            hideFlags = HideFlags.HideInInspector;
        }
    }

    // コマンド登録画面からの編集のみ可
    [CustomEditor(typeof(CommandTable))]
    public class CommandTableInspector : Editor {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("コマンド登録画面から編集してください");
        }
    }
}