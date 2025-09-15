using System;
using UnityEngine;

namespace Chonker.Runtime.Core.StateMachine
{
    public abstract class StateMachine<TStateId> : MonoBehaviour
        where TStateId : Enum
    {
        protected StateMachineManager<TStateId, StateMachine<TStateId>> ParentManager;
        public abstract TStateId StateId { get; }

        public abstract void Initialize();

        public abstract void OnEnter(TStateId prevState);

        public abstract void OnExit(TStateId newState);

        public void InitializeFields()
        {
            
        }
    }
}