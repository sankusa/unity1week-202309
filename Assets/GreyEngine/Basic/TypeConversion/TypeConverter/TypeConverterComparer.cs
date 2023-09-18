using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreyEngine.Basic.TypeConversion {
    public class TypeConverterComparer : IComparer<TypeConverter>
    {
        public int Compare(TypeConverter a, TypeConverter b) {
            int ret;
            if(a.Type.IsPrimitive != b.Type.IsPrimitive) {
                ret = a.Type.IsPrimitive ? -1 : 1;
            } else if((a.Type.Namespace == "UnityEngine") != (b.Type.Namespace == "UnityEngine")) {
                ret = a.Type.Namespace == "UnityEngine" ? -1 : 1;
            } else {
                ret = a.Type.FullName.CompareTo(b.Type.FullName);
            }
            return ret;
        }
    }
}