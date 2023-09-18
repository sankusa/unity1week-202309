using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SankusaLib.SoundLib {
    public class PlaySeMarker : Marker, INotification
    {
        [SerializeField, SoundId] private string soundId;
        public string SoundId => soundId;

        public PropertyName id {get;}
    }
}