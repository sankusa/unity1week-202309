using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GreyEngine.Basic {
    public abstract class TypeConverter
    {
        // 対象の型
        private Type type;
        public Type Type {
            get {return type;}
            protected set {
                type = value;
                simpleTypeName = type.Name;
            }
        }
        // UI表示用の型名(typeのsetterでtype.Nameが自動で設定されるが、必要があればtype設定後に上書きする)
        private string simpleTypeName;
        public string SimpleTypeName {
            get {return simpleTypeName;}
            protected set {simpleTypeName = value;}
        }
        // 初期化用文字列
        private string initialString;
        public string InitialString {
            get {return initialString;}
            protected set {initialString = value;}
        }
        // 変換関数(value → string)
        public abstract string ValueToString(object value);
        // 変換関数(string → value)
        public abstract object StringToValue(string valueString);
#if UNITY_EDITOR
        public abstract string Field(Rect rect, string label, string valueString);
        public abstract string FieldLayout(string label, string valueString);
#endif
    }
}