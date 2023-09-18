using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class IEnumerableExtension
{
    public static T GetRandom<T>(this IEnumerable<T> enumerable, float random){
        int count = enumerable.Count();
        int index = 0;
        int targetIndex = (int)(Mathf.Clamp01(random) * count);
        foreach(T t in enumerable) {
            if(index == targetIndex) return t;
            index++;
        }
        return enumerable.FirstOrDefault();
    }
}
