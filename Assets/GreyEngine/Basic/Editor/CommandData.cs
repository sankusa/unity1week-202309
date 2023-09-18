using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GreyEngine.Basic.Utils;

namespace GreyEngine.Basic {
    [System.Serializable]
    public class CommandData
    {
        public CommandCategory category;
        public string className;
        public string methodName;
        public List<string> argTypeNames;
        public List<string> argNames;
        public string returnTypeName;
        public string summary;
        public string customMenuPath;
        public Color color;
        public CommandData(CommandCategory category, string className, string methodName,
                           List<string> argTypeNames, List<string> argNames, string returnTypeName) {
            this.category = category;
            this.className = className;
            this.methodName = methodName;
            this.argTypeNames = argTypeNames;
            this.argNames = argNames;
            this.returnTypeName = returnTypeName;
            summary = "";
            customMenuPath = "";
            color = new Color(0, 0, 0, 0);
        }
        public CommandData(CommandData source) {
            this.category = source.category;
            this.className = source.className;
            this.methodName = source.methodName;
            this.argTypeNames = new List<string>(source.argTypeNames);
            this.argNames = new List<string>(source.argNames);
            this.returnTypeName = source.returnTypeName;
            this.summary = source.summary;
            this.customMenuPath = source.customMenuPath;
            this.color = source.color;
        }
        // メソッドキー(メソッド名、引数型によって一意に定める)
        public string GetMethodKey() {
            return CreateMethodKey(methodName, argTypeNames);
        }
        public static string CreateMethodKey(string methodName, List<string> argTypeNames) {
            string key = methodName + "(";
            for(int i = 0; i < argTypeNames.Count; i++) {
                if(i != 0) key += ",";
                key += argTypeNames[i];
            }
            key += ")";
            return key;
        }
        // メソッドラベル(GUI表示用)
        public string GetMethodLabel() {
            return CreateMethodLabel(methodName, argTypeNames, argNames);
        }
        public static string CreateMethodLabel(string methodName, List<string> argTypeNames, List<string> argNames) {
            string methodLabel;
            if(methodName == "") {
                methodLabel = "";
            } else {
                methodLabel = methodName + "(";
                for(int i = 0; i < argTypeNames.Count; i++) {
                    if(i != 0) methodLabel += ",  ";
                    methodLabel += UtilsForEditor.MasterConverter.GetConverter(argTypeNames[i]).SimpleTypeName + "  " + argNames[i];
                }
                if(argTypeNames.Count == 0) methodLabel += "void";
                methodLabel += ")";
            }
            return methodLabel;
        }
        // パス(メニュー用)
        public string GetPath() {
            string path = "All/" + category.ToString() + "/";
            string[] names = className.Split('.');
            for(int i = 0; i < names.Length; i++) {
                if(i == names.Length - 1) {
                    path += names[i] + "/";
                } else {
                    path += names[i] + "/";
                }
            }
            path += GetMethodLabel();

            return path;
        }
        // コマンド生成
        public Command CreateCommand() {
            return new Command(category, className, methodName, argTypeNames, UtilsForEditor.MasterConverter.GetInitialStrings(argTypeNames), returnTypeName);
        }
        // サマリ生成
        public string CreateSummary(Command command) {
            string ret = summary;
            // サマリ未入力時
            if(ret == "") {
                ret += className.Split('.')[className.Split('.').Length - 1] + "." + methodName + "(";
                for(int i = 0; i < argTypeNames.Count; i++) {
                    if(i != 0) ret += ", ";
                    ret +="{" + i + "}";
                }
                ret += ")";
            }
            // {0}→0番目の引数の内容で置換、{引数名}→対象の引数の内容で置換
            for(int i = 0; i < argTypeNames.Count; i++) {
                string value;
                if(command.argVariableUseFlags[i]) {
                    value = "<" + command.argVariableNames[i] + ">";
                } else {
                    value = UtilsForEditor.MasterConverter.GetConverter(argTypeNames[i]).StringToValue(command.argValueStrings[i]).ToString();
                }
                ret = ret.Replace("{" + i + "}", value);
                ret = ret.Replace("{" + argNames[i] + "}", value);
            }
            // [文字列]:[A][文字列がAの場合、置換する値]:[][]
            while(true) {
                int state = 0;
                int start = 0;
                int end = 0;
                string oldWord = "";
                string tmpKeyWord = "";
                string tmpNewWord = "";
                List<string> keyWords= new List<string>();
                List<string> newWords= new List<string>();
                string newWord = "";
                for(int i = 0; i < ret.Length; i++) {
                    if(state == 0) {
                        if(ret[i] == '[') {
                            start = i;
                            state = 1;
                        }
                    } else if(state == 1) {
                        if(ret[i] == ']') state = 2;
                        else oldWord += ret[i];
                    } else if(state == 2) {
                        if(ret[i] == ':') state = 3;
                        else break;
                    } else if(state == 3) {
                        if(ret[i] == '[') state = 4;
                        else break;
                    } else if(state == 4) {
                        if(ret[i] == ']') state = 5;
                        else tmpKeyWord += ret[i];
                    } else if(state == 5) {
                        if(ret[i] == '[') state = 6;
                        else break;
                    } else if(state == 6) {
                        if(ret[i] == ']') {
                            state = 7;
                            end = i;
                            keyWords.Add(tmpKeyWord);
                            newWords.Add(tmpNewWord);
                            tmpKeyWord = "";
                            tmpNewWord = "";
                        }
                        else tmpNewWord += ret[i];
                    } else if(state == 7) {
                        if(ret[i] == ':') {
                            state = 3;
                        } else break;
                    }
                }

                if(state == 7) {
                    for(int i = 0; i < keyWords.Count; i++) {
                        if(oldWord == keyWords[i]) {
                            newWord = newWords[i];
                            break;
                        }
                    }
                    ret = ret.Replace(ret.Substring(start, end - start + 1), newWord);
                } else {
                    break;
                }
            }



            // 改行をスペースに置換
            ret = ret.Replace("\n", "  ");
            return ret;
        }
    }
}