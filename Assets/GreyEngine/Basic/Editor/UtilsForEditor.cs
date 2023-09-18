using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using GreyEngine.Basic.TypeConversion;
using System;
using System.Linq;
using System.Reflection;

namespace GreyEngine.Basic {
    public class UtilsForEditor
    {
        // タイプコンバーター
        private static MasterTypeConverter masterConverter;
        public static MasterTypeConverter MasterConverter {
            get {
                if(masterConverter == null) {
                    masterConverter = MasterTypeConverter.CreateInstance();
                }
                return masterConverter;
            }
        }
        [DidReloadScripts]
        static void Reload() {
            if(masterConverter == null) {
                masterConverter = MasterTypeConverter.CreateInstance();
            }
        }
    }
}