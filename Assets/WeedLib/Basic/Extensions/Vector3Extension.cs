using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeedLib
{
    public static class Vector3Extension
    {
        public static Vector3 OverWrite(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                x ?? vector.x,
                y ?? vector.y,
                z ?? vector.z
            );
        }
        public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                vector.x + x.GetValueOrDefault(),
                vector.y + x.GetValueOrDefault(),
                vector.z + y.GetValueOrDefault()
            );
        }
    }
}