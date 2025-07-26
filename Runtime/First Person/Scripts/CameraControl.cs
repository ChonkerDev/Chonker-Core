using Chonker.Core.Scripts.Input;
using UnityEngine;

namespace Chonker.Core.FirstPerson
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private PlayerLookRotationTracker _rotationTracker;

        private void Update() {
            transform.eulerAngles = _rotationTracker.transform.eulerAngles;
        }

        public Vector3 ProjectInputOnLookView(Vector2 input) {
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = transform.right;
            right.y = 0;
            right.Normalize();

            return (right * input.x + forward * input.y).normalized;
        }

    }
}
