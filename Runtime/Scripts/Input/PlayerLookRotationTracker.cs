using UnityEngine;

namespace Chonker.Core.Scripts.Input
{
    public class PlayerLookRotationTracker : MonoBehaviour
    {
        [SerializeField, Range(0, 89)] private float UpperLookLimit = 60f;
        [SerializeField, Range(0, 89)] private float LowerLookLimit = 30f;
        [SerializeField] private float smoothTime = 0.05f; // smaller => faster
        [SerializeField] private PlayerActionInputManager _playerActionInputManager;
    
        private float yaw = 0f;
        private float pitch = 0f;

        private float yawVelocity = 0f;
        private float pitchVelocity = 0f;

        public float Yaw => yaw;
        public float Pitch => pitch;

        private void Update() {
            UpdateRotation();
        }

        public void UpdateRotation() {
            Vector2 lookInput = _playerActionInputManager.ReadLookInputDelta();

            float targetYaw = yaw + lookInput.x;
            float targetPitch = Mathf.Clamp(pitch - lookInput.y, -UpperLookLimit, LowerLookLimit);

            yaw = Mathf.SmoothDamp(yaw, targetYaw, ref yawVelocity, smoothTime);
            pitch = Mathf.SmoothDamp(pitch, targetPitch, ref pitchVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        
    
    }
}