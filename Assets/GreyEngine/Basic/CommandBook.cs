using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace GreyEngine.Basic {
    [System.Serializable, CreateAssetMenu(menuName = "GreyEngine/Create CommandBook")]
    public class CommandBook : ScriptableObject
    {
        public List<Command> commands = new List<Command>();
        public List<Variable> variables = new List<Variable>();
        public static CommandBook CreateBook() {
            CommandBook book = ScriptableObject.CreateInstance<CommandBook>();
            return book;
        }
    }
}