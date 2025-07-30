using System;
using UnityEngine;

namespace Chonker.Core
{
    public class CameraShakeAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private string lastPlayerState;

        private void crossFadeToState(string stateName, float duration = .2f) {
            if(lastPlayerState == stateName) return; // don't transition if already in or going to the same state
            lastPlayerState = stateName;
            _animator.CrossFade(stateName, duration);
        }

        public void UpdateVelocityParams(Vector3 normalizedVel) {
            _animator.SetFloat("Velocity Mag", normalizedVel.magnitude);
        }

        public void PlayNone() {
            crossFadeToState("Shake Layer.None");
        }

        public void PlayLocomotion() {
            crossFadeToState("Shake Layer.Locomotion");
        }

        public void PlayOneShotLand() {
            crossFadeToState("Oneshot Layer.Land");
        }
    }
}