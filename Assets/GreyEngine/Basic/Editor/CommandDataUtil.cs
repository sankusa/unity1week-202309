using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace GreyEngine.Basic {
    public class CommandDataUtil
    {
        // 全コマンド(アセンブリ内の全コンポーネントの全Publicメソッド)
        public static List<CommandData> GetCommands(Func<Type,bool> typeFilter, Func<MethodInfo,bool> methodFilter) {
            List<CommandData> commands = new List<CommandData>();
            // アセンブリ内全コンポーネント
            Type[] targetTypes = Assembly.GetAssembly(typeof(Command)).GetTypes().Where(typeFilter).ToArray();
            foreach(Type t in targetTypes) {
                MethodInfo[] markedMethods = t.GetMethods().Where(methodFilter).ToArray();
                foreach(MethodInfo m in markedMethods) {
                    // 引数型が変換可能かチェック
                    if(!isConvertibleMethod(m)) continue;
                    // 追加
                    commands.Add(CreateCommandData(CommandCategory.Normal, m));
                }
            }
            return commands;
        }

        // 分類、MethodInfoを元にCommandDataを生成
        public static CommandData CreateCommandData(CommandCategory category, MethodInfo method) {
            List<string> argTypeNames = new List<string>();
            List<string> argNames = new List<string>();
            foreach(ParameterInfo p in method.GetParameters()) {
                argTypeNames.Add(p.ParameterType.FullName);
                argNames.Add(p.Name);
            }
            // 戻り値型取得
            string returnTypeName = method.ReturnType.FullName;
            return new CommandData(category, method.ReflectedType.FullName, method.Name, argTypeNames, argNames, returnTypeName);
        }

        // メソッドの全引数が変換可能な型かどうか
        public static bool isConvertibleMethod(MethodInfo method) {
            bool isValid = true;
            foreach(ParameterInfo p in method.GetParameters()) {
                if(UtilsForEditor.MasterConverter.isConvertibleType(p.ParameterType) == false) {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
    }
}