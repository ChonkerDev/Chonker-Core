using System;
using UnityEngine;

namespace Chonker.Runtime.Core.StateMachine {
    public abstract class StateMachine : MonoBehaviour {
        public abstract string StateId { get; }

        public abstract void Initialize();

        public abstract void OnEnter(string prevState, bool calledFromStart = false);

        public abstract void OnExit(string newState);

        public void InitializeFields() {
        }
    }
}