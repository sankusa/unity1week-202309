using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.StateMachineLib {
    public class EmptyState : IState {
        public void OnEnter(params object[] _){}
        public void Update(){}
        public void OnExit(){}
    }
}
