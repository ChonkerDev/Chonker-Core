using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chonker.Scripts.Core
{

    public abstract class AnimationLayerManager<TLayerTypeEnum> : MonoBehaviour where TLayerTypeEnum : Enum
    {
        protected Animator animator;
        protected abstract TLayerTypeEnum initialStateType { get; }
        private TLayerTypeEnum lastStateType;
        public int LayerIndex { get; private set; }
        public abstract string animatorLayerName { get; }
        
        public void Initialize(Animator animator) {
            this.animator = animator;
            LayerIndex = animator.GetLayerIndex(animatorLayerName);
            CrossFadeAnimationState(initialStateType, 0);
            lastStateType = initialStateType;
        }

        protected void CrossFadeAnimationState(TLayerTypeEnum stateId, float fixedTransitionDuration = .2f) {
            if (Equals(stateId, lastStateType)) return; // don't transition if already in or going state
            lastStateType = stateId;
            int stateHash = getAnimationHash(stateId);
            animator.CrossFadeInFixedTime(stateHash, fixedTransitionDuration, LayerIndex);
        }

        protected abstract int getAnimationHash(TLayerTypeEnum stateId);
        
        public bool CompareCurrentAndNextState(TLayerTypeEnum stateId) {
            AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(LayerIndex);
            AnimatorStateInfo next = animator.GetNextAnimatorStateInfo(LayerIndex);

            int currentStateHash = getAnimationHash(stateId);
            return current.fullPathHash == currentStateHash || next.fullPathHash == currentStateHash;;
        }
    }
}