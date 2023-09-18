using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UniRx;

namespace WeedLib.SerializableStateMachine {
    [Serializable]
    public class StateMachine<TOwner, StateTypeEnum> where StateTypeEnum : Enum {
        protected Dictionary<StateTypeEnum, StateBase<TOwner>> _states = new Dictionary<StateTypeEnum, StateBase<TOwner>>();
        public IReadOnlyDictionary<StateTypeEnum, StateBase<TOwner>> States => _states;

        [SerializeField] protected ReactiveProperty<StateTypeEnum> _currentStateTypeReactiveProperty = new ReactiveProperty<StateTypeEnum>();
        public StateTypeEnum CurrentStateType {
            get => _currentStateTypeReactiveProperty.Value;
            private set => _currentStateTypeReactiveProperty.Value = value;
        }
        public IObservable<StateTypeEnum> OnCurrentStateTypeChanged => _currentStateTypeReactiveProperty;

        private BehaviorSubject<StateTypeEnum> _onEnteredStateSubject = new BehaviorSubject<StateTypeEnum>(default(StateTypeEnum));
        public IObservable<StateTypeEnum> OnEnteredState => _onEnteredStateSubject;

        public StateBase<TOwner> CurrentState {
            get {
                if(_states.Count > 0 && !_states.ContainsKey(CurrentStateType)) {
                    Debug.LogWarning($"{nameof(CurrentStateType)} is invalid. value = {CurrentStateType}");
                }
                return _states.ContainsKey(CurrentStateType) ? _states[CurrentStateType] : null;
            }
        }

        [NonSerialized] protected TOwner _owner;
        public TOwner Owner => _owner;

        protected StateMachine() {
            
        }

        public StateMachine(StateTypeEnum initialStateType) {
            CurrentStateType = initialStateType;
        }

        public void Update(float deltaTime) {
            CurrentState?.OnUpdate(deltaTime);
        }

        /// <summary>
        /// ステート構築後に呼ぶ
        /// </summary>
        public void SetUp(TOwner owner) {
            _owner = owner;
            foreach(var state in _states.Values) {
                state.SetUp(owner);
            }

            _onEnteredStateSubject.OnNext(CurrentStateType);
        }

        public void AddState(StateTypeEnum stateType, StateBase<TOwner> state) {
            _states[stateType] = state;
        }

        public void ChangeState(StateTypeEnum stateType) {
            CurrentState?.OnExit();
            CurrentStateType = stateType;
            CurrentState?.OnEnter();
            _onEnteredStateSubject.OnNext(CurrentStateType);
        }

        public void InvokeOnEnter() {
            CurrentState?.OnEnter();
        }

        public void ChangeStateCalm(StateTypeEnum stateType) {
            CurrentStateType = stateType;
        }
    }

    [Serializable]
    public class StateMachine<TOwner, StateTypeEnum, TriggerTypeEnum> : StateMachine<TOwner, StateTypeEnum> where StateTypeEnum : Enum where TriggerTypeEnum : Enum {
        protected Dictionary<StateTypeEnum, List<Transition<StateTypeEnum, TriggerTypeEnum>>> _transitionListDic = new Dictionary<StateTypeEnum, List<Transition<StateTypeEnum, TriggerTypeEnum>>>();

        protected StateMachine() {
            
        }

        public StateMachine(StateTypeEnum initialState) : base(initialState) {

        }

        public void ExecuteTrigger(TriggerTypeEnum triggerType) {
            List<Transition<StateTypeEnum, TriggerTypeEnum>> transitions = _transitionListDic[CurrentStateType];
            foreach(Transition<StateTypeEnum, TriggerTypeEnum> transition in transitions) {
                if (transition.TriggerType.Equals(triggerType)) {
                    ChangeState(transition.ToStateType);
                    break;
                }
            }
        }
    
        public void AddTransition(StateTypeEnum fromStateType, StateTypeEnum toStateType, TriggerTypeEnum triggerType) {
            if (!_transitionListDic.ContainsKey(fromStateType)) {
                _transitionListDic.Add(fromStateType, new List<Transition<StateTypeEnum, TriggerTypeEnum>>());
            }

            List<Transition<StateTypeEnum, TriggerTypeEnum>> transitions = _transitionListDic[fromStateType];

            Transition<StateTypeEnum, TriggerTypeEnum> transition = transitions.FirstOrDefault(x => x.ToStateType.Equals(toStateType));
            if (transition == null) {
                transitions.Add(new Transition<StateTypeEnum, TriggerTypeEnum>(toStateType, triggerType));
            }
            else {
                transition.ToStateType = toStateType;
                transition.TriggerType = triggerType;
            }
        }        
    }
}