using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.StateMachineLib {
    public interface IState {
        void OnEnter(params object[] args);
        void Update();
        void OnExit();
    }
}