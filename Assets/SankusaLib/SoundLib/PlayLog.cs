using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public class PlayLog
    {
        private List<PlayLogRecord> records = new List<PlayLogRecord>();
        public IReadOnlyList<PlayLogRecord> Records => records;
        private int capacity = 0;

        public PlayLog(int capacity) {
            this.capacity = capacity;
        }

        public void Add(PlayLogRecord record) {
            if(capacity == 0) return;
            records.Add(record);
            if(capacity == -1) return;
            if(records.Count > capacity) {
                records.RemoveAt(0);
            }
        }

        public void DeleteNewRecord(int deleteCount) {
            for(int i = 0; i < deleteCount; i++) {
                if(records.Count == 0) break;
                records.RemoveAt(records.Count - 1);
            }
        }

        public void Clear() {
            records.Clear();
        }
    }
}