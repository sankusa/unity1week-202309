using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.LocalizeLib {
    [CreateAssetMenu(menuName = nameof(SankusaLib) + "/" + nameof(LocalizeLib) + "/" + nameof(Localize), fileName = nameof(Localize))]
    public class Localize : ScriptableObject
    {
        // 自動ロード
        private static Localize instance;
        public static Localize Instance => instance;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load() {
            instance = Resources.Load<Localize>(nameof(Localize));
        }

        [SerializeField] private SystemLanguage defaultLanguage;
        public SystemLanguage DefaultLanguage => defaultLanguage;

        [SerializeField] List<TableContainer> tableContainers;

        public IReadOnlyList<string> GetAllStringKeys(SystemLanguage language) {
            TableContainer container = tableContainers.Find(x => x.Language == language);
            if(container == null) return new List<string>();
            return container.AllStringKeys;
        }

        public TableContainer GetTableContainer(SystemLanguage language) {
            TableContainer container = tableContainers.Find(x => x.Language == language);
            if(container == null) {
                Debug.LogWarning("target language not found. : " + language);
            }
            return container;
        }

        private TableContainer GetTableContainer() {
            if(tableContainers.Count == 0) return null;

            TableContainer container = tableContainers.Find(x => x.Language == Application.systemLanguage);
            if(container == null) {
                container = tableContainers.Find(x => x.Language == defaultLanguage);
                if(container == null) {
                    Debug.LogWarning("target language and default language not found.");
                    container = tableContainers[0];
                }
            }
            return container;
        }

        public string FindString(string key) {
            TableContainer container = GetTableContainer();
            string result = "";

            if(container != null) {
                result = container.FindString(key);
                if(result != null) {
                    return result;
                } else {
                    result = "String not found. key = " + key;
                    Debug.LogWarning(result);
                    return result;
                }
            } else {
                result = "TableContainer not found.";
                Debug.LogWarning(result);
                return result;
            }
        }

        public List<string> FindStrings(string key) {
            TableContainer container = GetTableContainer();

            if(container != null) {
                List<string> results = container.FindStrings(key);
                return results;
            } else {
                Debug.LogWarning("TableContainer not found.");
                return new List<string>();
            }
        }
    }
}