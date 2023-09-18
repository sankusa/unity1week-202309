using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.SoundLib {
    public class PlayLogRecord
    {
        private string soundId;
        public string SoundId => soundId;

        private bool loop;
        public bool Loop => loop;

        public PlayLogRecord(string soundId, bool loop) {
            this.soundId = soundId;
            this.loop = loop;
        }
    }
}