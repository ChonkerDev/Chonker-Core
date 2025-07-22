using UnityEngine;

namespace Chonker.Core.Animation
{
    public class AnimatorRootMotionTracker : MonoBehaviour
    {
        public Vector3 deltaPosition { get; private set; } = Vector3.zero;
        public Quaternion deltaRotation { get; private set; } = Quaternion.identity;
        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        void Start() {
            ClearRootMotion();
        }

        private void OnAnimatorMove() {
            deltaPosition += animator.deltaPosition;
            deltaRotation = animator.deltaRotation * deltaRotation;
        }

        public Vector3 ConsumeDeltaPosition() {
            Vector3 temp = deltaPosition;
            deltaPosition = Vector3.zero;
            return temp;
        }

        public void ClearRootMotion() {
            deltaPosition = Vector3.zero;
            deltaRotation = Quaternion.identity;
        }

        public void activate() {
            enabled = true;
            ClearRootMotion();
        }

        public void deactivate() {
            enabled = false;
        }
    }
}
