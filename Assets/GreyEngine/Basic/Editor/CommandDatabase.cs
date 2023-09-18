using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreyEngine.Basic.TypeConversion;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using GreyEngine.Basic.Utils;

namespace GreyEngine.Basic {
    public class CommandDatabase
    {
        public List<CommandData> datas = new List<CommandData>();
        public static CommandDatabase allData = new CommandDatabase();

        // コンパイル時、自動でalldataを再作成する
        [DidReloadScripts]
        private static void ReloadAllData() {
            allData.datas = CommandDataUtil.GetCommands(
                t => {return /*t.IsSubclassOf(typeof(MonoBehaviour)) && */!t.IsAbstract;},
                m => {return m.ReflectedType.Equals(m.DeclaringType) && m.IsPublic && !m.IsStatic && !m.IsVirtual &&!m.IsAbstract && !m.IsConstructor;}
            );
            allData.datas.Sort(new CommandDataComparer());
        }

        // 全CommandTableの内容をまとめる
        public static CommandDatabase LoadDatabase() {
            CommandDatabase database = new CommandDatabase();
            foreach(CommandTable t in AssetUtil.LoadAllAssets<CommandTable>()) {
                foreach(CommandData d in t.datas) {
                    database.datas.Add(new CommandData(d));
                }
            }
            database.datas.Sort(new CommandDataComparer());
            return database;
        }
        
        // 検索
        public CommandData FindCommandData(Command command) {
            string methodKey = CommandData.CreateMethodKey(command.methodName, command.argsTypeNames);
            foreach(CommandData d in datas) {
                if(d.GetMethodKey() == methodKey &&
                   d.className == command.className &&
                   d.category == command.category
                  ) {
                      return d;
                  }
            }
            return null;
        }
        
        // 存在チェック
        public bool Exists(CommandData data) {
            foreach(CommandData d in datas) {
                if(d.category == data.category &&
                  d.className == data.className &&
                  d.GetMethodKey() == data.GetMethodKey()) {
                      return true;
                  }
            }
            return false;
        }
    }
}
