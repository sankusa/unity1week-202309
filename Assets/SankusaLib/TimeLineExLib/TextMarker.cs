using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TextMarker : Marker, INotification
{
    [SerializeField] private string message;
    public string Message => message;

    public PropertyName id {
        get => new PropertyName("method");
    }
}
