using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace SankusaLib.SoundLib {
    public class PlaySeMarkerReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context) {
            PlaySeMarker marker = notification as PlaySeMarker;
            if(marker == null) return;

#if UNITY_EDITOR
    if(UnityEditor.EditorApplication.isPlaying) SoundManager.Instance.PlaySe(marker.SoundId);
#else
    SoundManager.Instance.PlaySe(marker.SoundId);
#endif
        }
    }
}