using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreyEngine.Basic.TypeConversion;

namespace GreyEngine.Basic {
    public enum CommandCategory {
        Normal = 10,
        BookControl = 20
    }
    // コマンド情報格納クラス
    [System.Serializable]
    public class Command
    {
        public CommandCategory category;
        public string className;
        public string methodName;
        public List<string> argsTypeNames;
        public List<string> argValueStrings;
        public List<bool> argVariableUseFlags;
        public List<string> argVariableNames;
        public string returnTypeName;
        public string returnSaveVariableName;
        public bool executeStop;
        public string tag;
        public bool waitUntil;
        public string waitConditionValueString;
        public Command(CommandCategory category, string className, string methodName, List<string> argsTypeNames, List<string> argValueStrings, string returnTypeName) {
            this.category = category;
            this.className = className;
            this.methodName = methodName;
            this.argsTypeNames = argsTypeNames;
            this.argValueStrings = argValueStrings;
            argVariableUseFlags = new List<bool>();
            argVariableNames = new List<string>();
            foreach(string arg in argsTypeNames) {
                argVariableUseFlags.Add(false);
                argVariableNames.Add("");
            }
            this.returnTypeName = returnTypeName;
            returnSaveVariableName = "";
            executeStop = false;
            tag = "";
            waitUntil = false;
            waitConditionValueString = "";
        }
        public Command(Command source) {
            this.category = source.category;
            this.className = source.className;
            this.methodName = source.methodName;
            this.argsTypeNames = new List<string>(source.argsTypeNames);
            this.argValueStrings = new List<string>(source.argValueStrings);
            this.argVariableUseFlags = new List<bool>(source.argVariableUseFlags);
            this.argVariableNames = new List<string>(source.argVariableNames);
            this.returnTypeName = source.returnTypeName;
            this.returnSaveVariableName = source.returnSaveVariableName;
            this.executeStop = source.executeStop;
            this.tag = source.tag;
            this.waitUntil = source.waitUntil;
            this.waitConditionValueString = source.waitConditionValueString;
        }
    }
}