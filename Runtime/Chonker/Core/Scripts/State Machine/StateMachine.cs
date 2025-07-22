using System;
using UnityEngine;

namespace Chonker.Scripts.Core.StateMachine
{
    public abstract class StateMachine<TStateId> : MonoBehaviour where TStateId : Enum
    {
        public abstract TStateId StateId { get; }

        public abstract void Initialize();

        public abstract void OnEnter();

        public abstract void OnExit();
    }
}