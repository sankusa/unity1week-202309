using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SankusaLib.StateMachineLib {
    public class StateMachine
    {
        private Dictionary<string, IState> states = new Dictionary<string, IState>();
        private IState currentState = new EmptyState();

        public void Update() {
            currentState.Update();
        }

        public void Change(string stateName, params object[] args) {
            currentState.OnExit();
            currentState = states[stateName];
            currentState.OnEnter(args);
        }

        public void ChangeEmptyState() {
            currentState = new EmptyState();
        }

        public void Add(string stateName, IState state) {
            states[stateName] = state;
        }
    }
}