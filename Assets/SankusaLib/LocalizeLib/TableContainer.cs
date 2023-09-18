using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SankusaLib.LocalizeLib {
    [System.Serializable]
    public class TableContainer
    {
        public SystemLanguage Language => language;
        [SerializeField] private SystemLanguage language;

        [SerializeField] private List<StringTable> stringTables;

        public IReadOnlyList<string> AllStringKeys => stringTables.SelectMany(x => x.Keys).ToArray();

        public string FindString(string key) {
            foreach(StringTable table in stringTables) {
                string value = table.FindString(key);
                if(value != null) return value;
            }
            return null;
        }

        public List<string> FindStrings(string key) {
            List<string> results = new List<string>();
            foreach(StringTable table in stringTables) {
                results.AddRange(table.FindStrings(key));
            }
            return results;
        }
    }
}