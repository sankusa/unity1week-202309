using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.LocalizeLib {
    [System.Serializable]
    public class StringData
    {
        [SerializeField] private string key;
        public string Key => key;

        [SerializeField] private string value;
        public string Value => value;
    }
}