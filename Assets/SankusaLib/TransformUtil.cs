using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SankusaLib
{
    public static class TransformUtil
    {
        public static List<T> AdjustChildCount<T>(Transform parent, GameObject prefab, int count) where T : MonoBehaviour
        {
            AdjustChildCount(parent, prefab, count);

            List<T> list = new List<T>();
            foreach(Transform t in parent)
            {
                list.Add(t.GetComponent<T>());
            }

            return list;
        }

        public static void AdjustChildCount(Transform parent, GameObject prefab, int count)
        {
            int childCount = parent.childCount;
            int countDiff = count - childCount;
            if(countDiff > 0)
            {
                for(int i = 0; i < countDiff; i++)
                {
                    GameObject.Instantiate(prefab, parent);
                }
            }
            else if(countDiff < 0)
            {
                for(int i = childCount - 1; i >= childCount + countDiff; i--)
                {
                    GameObject.Destroy(parent.GetChild(i).gameObject);
                }
            }
        }
    }
}