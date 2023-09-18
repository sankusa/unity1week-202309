using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WeedLib.SimpleMessage
{
    public static class SimpleMessageCore
    {
        private static UnityEvent<string> onSetMessage = new UnityEvent<string>();
        public static UnityEvent<string> OnSetMessage => onSetMessage;
        public static void Set(string message)
        {
            onSetMessage.Invoke(message);
        }
    }
}