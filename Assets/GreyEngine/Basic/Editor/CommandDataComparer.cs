using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GreyEngine.Basic {
    public class CommandDataComparer : IComparer<CommandData>
    {
        public int Compare(CommandData a, CommandData b) {
            int ret;
            if(a.category != b.category) {
                ret = b.category - a.category;
            } else if(a.className != b.className) {
                ret = a.className.CompareTo(b.className);
            } else {
                ret = a.GetMethodKey().CompareTo(b.GetMethodKey());
            }
            return ret;
        }
    }
}