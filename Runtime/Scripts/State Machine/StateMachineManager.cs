using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Chonker.Runtime.Core.StateMachine {
    public abstract class StateMachineManager<TState> : MonoBehaviour
        where TState : StateMachine {
        
        [SerializeField] private string _currentState;
        [SerializeField] private bool debugDraw;
        public string CurrentState => _currentState;

        private Dictionary<string, TState> states;

        public UnityEvent<string, string> OnStateChange = new();

        protected virtual void OnAwake() {
        }

        protected virtual void OnStart() {
        }

        protected abstract string InitialState();

        [Obsolete("Do not override Awake. Use OnAwake() instead.", true)]
        private void Awake() {
            FindAndInitializeStates();
            _currentState = InitialState();
            OnAwake();
        }

        [Obsolete("Do not override Start. Use OnStart() instead.", true)]
        private void Start() {
            GetCurrentState().OnEnter(_currentState, true);
            OnStart();
        }

        private void FindAndInitializeStates() {
            states = new();
            foreach (TState stateMachine in GetComponentsInChildren<TState>()) {
                stateMachine.Initialize();
                stateMachine.InitializeFields();
                states.Add(stateMachine.StateId, stateMachine);
            }
        }

        public TState GetCurrentState() {
            return states[CurrentState];
        }

        public TState GetState(string stateId) {
            return states[stateId];
        }

        protected void UpdateState(string stateId) {
            if (debugDraw) {
                Debug.Log($"Updated to {stateId}");
            }

            string prevState = _currentState;
            OnStateChange.Invoke(_currentState, stateId);
            GetCurrentState().OnExit(stateId);
            _currentState = stateId;
            GetCurrentState().OnEnter(prevState);
        }
    }
}