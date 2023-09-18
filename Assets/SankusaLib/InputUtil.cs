using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SankusaLib {
    public class InputUtil
    {
        // EventSystem.current.IsPointerOverGameObject(touch.fingerId)
        // がうまく動かないので、その代わり
        private static List<RaycastResult> raycastResults = new List<RaycastResult>();  // リストを使いまわす
        public static bool IsPointerOverUIObject(Vector2 screenPosition) {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            EventSystem.current.RaycastAll(eventDataCurrentPosition, raycastResults);
            var over = raycastResults.Count > 0;
            raycastResults.Clear();
            return over;
        }
    }
}