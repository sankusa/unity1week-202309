using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

namespace WeedLib.SerializableStateMachine {
    [Serializable]
    public abstract class StateBase<TOwner> {
        protected TOwner _owner;
        public virtual void SetUp(TOwner owner)
        {
            _owner = owner;
        }
        public virtual void OnEnter(){}
        public virtual void OnUpdate(float deltaTime){}
        public virtual void OnExit(){}
    }

    // サブステート付き
    public abstract class StateBase<TOwner, StateTypeEnum> : StateBase<TOwner> where StateTypeEnum : Enum {
        [SerializeField] protected StateMachine<TOwner, StateTypeEnum> subStateMachine;
        public StateMachine<TOwner, StateTypeEnum> SubStateMachine => subStateMachine;

        private bool useDefaultStateType = false;
        private StateTypeEnum defaultStateType;

        protected StateBase() {
            
        }

        public StateBase(StateTypeEnum initialStateType) {
            subStateMachine = new StateMachine<TOwner, StateTypeEnum>(initialStateType);
        }

        public override void SetUp(TOwner owner) {
            base.SetUp(owner);
            subStateMachine.SetUp(owner);
        }

        public void SetDefaultStateType(StateTypeEnum defaultStateType) {
            useDefaultStateType = true;
            this.defaultStateType = defaultStateType;
        }

        public void ClearDefaultStateType() {
            useDefaultStateType = false;
            defaultStateType = default(StateTypeEnum);
        }

        public override void OnEnter() {
            if(useDefaultStateType) subStateMachine.ChangeStateCalm(defaultStateType);
            subStateMachine.CurrentState.OnEnter();
        }

        public override void OnUpdate(float deltaTime) {
            subStateMachine.CurrentState.OnUpdate(deltaTime);
        }

        public override void OnExit() {
            subStateMachine.CurrentState.OnExit();
            if(useDefaultStateType) subStateMachine.ChangeStateCalm(defaultStateType);
        }
    }

    // サブステート&トリガー付き
    public abstract class StateBase<TOwner, StateTypeEnum, TriggerTypeEnum> : StateBase<TOwner> where StateTypeEnum : Enum where TriggerTypeEnum : Enum {
        // サブステート
        [SerializeField] protected StateMachine<TOwner, StateTypeEnum, TriggerTypeEnum> subStateMachine;
        public StateMachine<TOwner, StateTypeEnum, TriggerTypeEnum> SubStateMachine => subStateMachine;

        private bool useDefaultStateType = false;
        private StateTypeEnum defaultStateType;

        protected StateBase() {
            
        }

        public StateBase(StateTypeEnum initialStateType) {
            subStateMachine = new StateMachine<TOwner, StateTypeEnum, TriggerTypeEnum>(initialStateType);
        }

        public void SetDefaultStateType(StateTypeEnum defaultStateType) {
            useDefaultStateType = true;
            this.defaultStateType = defaultStateType;
        }

        public void ClearDefaultStateType() {
            useDefaultStateType = false;
            defaultStateType = default(StateTypeEnum);
        }

        public override void OnEnter() {
            if(useDefaultStateType) subStateMachine.ChangeStateCalm(defaultStateType);
            subStateMachine.CurrentState.OnEnter();
        }

        public override void OnUpdate(float deltaTime) {
            subStateMachine.CurrentState.OnUpdate(deltaTime);
        }

        public override void OnExit() {
            subStateMachine.CurrentState.OnExit();
            if(useDefaultStateType) subStateMachine.ChangeStateCalm(defaultStateType);
        }
    }
}