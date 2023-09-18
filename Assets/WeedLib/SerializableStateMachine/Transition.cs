using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeedLib {
    public class Transition<StateTypeEnum, TriggerTypeEnum> {
        private StateTypeEnum toStateType;
        public StateTypeEnum ToStateType {
            get => toStateType;
            set => toStateType = value;
        }

        private TriggerTypeEnum triggerType;
        public TriggerTypeEnum TriggerType {
            get => triggerType;
            set => triggerType = value;
        }

        public Transition(StateTypeEnum toStateType, TriggerTypeEnum triggerType) {
            this.toStateType = toStateType;
            this.triggerType = triggerType;
        }
    }
}