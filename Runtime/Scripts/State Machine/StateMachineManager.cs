using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chonker.Runtime.Core.StateMachine
{
    public abstract class StateMachineManager<TStateId, TState> : MonoBehaviour
        where TStateId : Enum
        where TState : StateMachine<TStateId>
    {
        [SerializeField] private TStateId _initialState;
        [SerializeField] private TStateId _currentState;
        [SerializeField] private bool debugDraw;
        public TStateId CurrentState => _currentState;

        private Dictionary<TStateId, TState> states;

        protected virtual void OnAwake() {
            
        }

        protected virtual void OnStart() {
            
        }
        
        [Obsolete("Do not override Awake. Use OnAwake() instead.", true)]
        private void Awake() {
            FindAndInitializeStates();
            OnAwake();
        }

        [Obsolete("Do not override Start. Use OnStart() instead.", true)]
        private void Start() {
            _currentState = _initialState;
            processOnEnter();
            OnStart();
        }

        private void FindAndInitializeStates() {
            states = new();
            foreach (TState stateMachine in GetComponentsInChildren<TState>()) {
                stateMachine.Initialize();
                states.Add(stateMachine.StateId, stateMachine);
            }
        }

        public TState GetCurrentState() {
            return states[CurrentState];
        }

        public TState GetState(TStateId stateId) {
            return states[stateId];
        }

        protected void UpdateState(TStateId stateId) {
            if (debugDraw) {
                Debug.Log($"Updated to {stateId}");
            }
            GetCurrentState().OnExit();
            _currentState = stateId;
            processOnEnter();
        }

        private void processOnEnter() {
            GetCurrentState().OnEnter();
        }
        
    }
}