using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class TextMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private UnityEvent<string> events = new UnityEvent<string>();

    public void OnNotify(Playable origin, INotification notification, object context) {
        TextMarker marker = notification as TextMarker;
        if(marker == null) return;

        events.Invoke(marker.Message);
    }
}
