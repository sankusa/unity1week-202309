using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SankusaLib.CustomPopupLib {
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class AssetPathPopupAttribute : PropertyAttribute
    {
        public readonly Type type;
        public readonly bool underResources;
        public AssetPathPopupAttribute(Type type, bool underResources) {
            this.type = type;
            this.underResources = underResources;
        }
    }
}