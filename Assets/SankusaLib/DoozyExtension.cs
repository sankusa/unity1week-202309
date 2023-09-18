using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Triggers;
using UnityEngine.Events;

namespace SankusaLib {
    public static class DoozyExtension {
        // イベント取得
        private static UnityEvent SafeGetBehaviourEvent<Trigger>(this UIButton button, UIBehaviour.Name name)
        {
            UIBehaviour behaviour = button.behaviours.GetBehaviour(name);
            if(behaviour == null) {
                button.behaviours.AddBehaviour(name);
            }
            Trigger trigger = button.gameObject.GetComponent<Trigger>();
            if(trigger == null) {
                button.gameObject.AddComponent(typeof(Trigger));
            }
            return button.behaviours.GetBehaviour(name).Event;
        }
        public static UnityEvent SafeGetPointerClickEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<PointerClickTrigger>(UIBehaviour.Name.PointerClick);
        }
        public static UnityEvent SafeGetPointerEnterEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<PointerEnterTrigger>(UIBehaviour.Name.PointerEnter);
        }
        public static UnityEvent SafeGetPointerExitEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<PointerExitTrigger>(UIBehaviour.Name.PointerExit);
        }
        public static UnityEvent SafeGetPointerDownEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<PointerDownTrigger>(UIBehaviour.Name.PointerDown);
        }
        public static UnityEvent SafeGetPointerUpEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<PointerUpTrigger>(UIBehaviour.Name.PointerUp);
        }
        public static UnityEvent SafeGetSelectedEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<UISelectedTrigger>(UIBehaviour.Name.Selected);
        }
        public static UnityEvent SafeGetDeselectedEvent(this UIButton button)
        {
            return button.SafeGetBehaviourEvent<UIDeselectedTrigger>(UIBehaviour.Name.Deselected);
        }

        // イベント登録
        private static void AddListenerToUIBehaviour<Trigger>(this UIButton button, UnityAction action, UIBehaviour.Name name) {
            button.SafeGetBehaviourEvent<Trigger>(name).AddListener(action);
        }

        public static void AddListenerToPointerClick(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<PointerClickTrigger>(action, UIBehaviour.Name.PointerClick);
        }

        public static void AddListenerToPointerEnter(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<PointerEnterTrigger>(action, UIBehaviour.Name.PointerEnter);
        }

        public static void AddListenerToPointerExit(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<PointerExitTrigger>(action, UIBehaviour.Name.PointerExit);
        }

        public static void AddListenerToPointerDown(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<PointerDownTrigger>(action, UIBehaviour.Name.PointerDown);
        }

        public static void AddListenerToPointerUp(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<PointerUpTrigger>(action, UIBehaviour.Name.PointerUp);
        }

        public static void AddListenerToSelected(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<UISelectedTrigger>(action, UIBehaviour.Name.Selected);
        }

        public static void AddListenerToDeselected(this UIButton button, UnityAction action) {
            button.AddListenerToUIBehaviour<UIDeselectedTrigger>(action, UIBehaviour.Name.Deselected);
        }
    }
}