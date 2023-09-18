using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace GreyEngine.Basic.TypeConversion {
    public class MasterTypeConverter
    {
        public List<TypeConverter> converters;
        // インスタンスの取得ははCreateInstanceにより行う。
        private MasterTypeConverter() {}
        public static MasterTypeConverter CreateInstance() {
            MasterTypeConverter instance = new MasterTypeConverter();
            // リスト初期化
            instance.converters = new List<TypeConverter>();
            // TypeConverterのサブクラスをリフレクションで全取得
            List<Type> types = Assembly.GetAssembly(typeof(TypeConverter)).GetTypes()
            .Where(t => {return t.IsSubclassOf(typeof(TypeConverter)) == true;})
            .ToList();
            // ソート
            instance.converters.Sort(new TypeConverterComparer());
            // 取得したTypeConverterのサブクラスをインスタンス化
            foreach(Type t in types) {
                instance.converters.Add((TypeConverter) Activator.CreateInstance(t));
            }
            return instance;
        }
        // コンバータ検索
        public TypeConverter GetConverter(Type type) {
            foreach(TypeConverter converter in converters) {
                if(type.Equals(converter.Type)) return converter;
            }
            return null;
        }
        public TypeConverter GetConverter(string typeName) {
            foreach(TypeConverter converter in converters) {
                if(typeName == converter.Type.FullName) return converter;
            }
            return null;
        }
        // 初期値取得
        public string GetInitialString(string typeName) {
            return GetConverter(typeName).InitialString;
        }
        public List<string> GetInitialStrings(List<string> typeNames) {
            List<string> initialStrings = new List<string>();
            foreach(string typeName in typeNames) {
                initialStrings.Add(GetInitialString(typeName));
            }
            return initialStrings;
        }
        // 変換
        public object Convert(string typeName, string valueString) {
            return GetConverter(typeName).StringToValue(valueString);
        }
        // 使用可能な型か判定
        public bool isConvertibleType(Type type) {
            bool ret = false;
            foreach(TypeConverter converter in converters) {
                if(type.Equals(converter.Type)) {
                    ret = true;
                    break;
                }
            }
            return ret;
        }
        public bool isConvertibleType(string typeName) {
            bool ret = false;
            foreach(TypeConverter converter in converters) {
                if(typeName == converter.Type.FullName) {
                    ret = true;
                    break;
                }
            }
            return ret;
        }
    }
}