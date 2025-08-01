using UnityEngine;

namespace Chonker.Core.Scripts.Input
{
    /*
     * for tracking player look rotation with a transform for visualization
     */
    public class PlayerLookRotationTracker : MonoBehaviour
    {
        [SerializeField, Range(0, 89)] private int UpperLookLimit = 80;
        [SerializeField, Range(0, 89)] private int LowerLookLimit = 80;
        [SerializeField] private float smoothTime = 0.05f; // smaller => faster
    
        private float yaw = 0f;
        private float pitch = 0f;

        private float yawVelocity = 0f;
        private float pitchVelocity = 0f;

        public float Yaw => yaw;
        public float Pitch => pitch;

        /*
         * needs to be called in Update or LateUpdate in another script
         */
        public void UpdateRotation(Vector2 lookInputDelta) {
            Vector2 lookInput = lookInputDelta;

            float targetYaw = yaw + lookInput.x;
            float targetPitch = Mathf.Clamp(pitch - lookInput.y, -UpperLookLimit, LowerLookLimit);

            yaw = Mathf.SmoothDamp(yaw, targetYaw, ref yawVelocity, smoothTime);
            pitch = Mathf.SmoothDamp(pitch, targetPitch, ref pitchVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
        
    
    }
}