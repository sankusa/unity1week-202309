using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SankusaLib.LocalizeLib {
    [CreateAssetMenu(menuName = nameof(SankusaLib) + "/" + nameof(LocalizeLib) + "/" + nameof(StringTable), fileName = nameof(StringTable))]
    public class StringTable : ScriptableObject
    {
        [SerializeField] private List<StringData> dataList;

        public IReadOnlyList<string> Keys => dataList.Select(x => x.Key).ToList();

        public string FindString(string key) {
            StringData data = dataList.Find(x => x.Key == key);
            if(data != null) {
                return data.Value;
            } else {
                return "----";
            }
        }

        public List<string> FindStrings(string key) {
            List<string> results = new List<string>();
            foreach(StringData data in dataList.Where(x => x.Key.IndexOf(key) != -1)) {
                results.Add(data.Value);
            }
            return results;
        }
    }
}