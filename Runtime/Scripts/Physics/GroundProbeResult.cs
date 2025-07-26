using UnityEngine;

namespace Chonker.Core.Scripts.Physics
{
    public class GroundProbeResult
    {
        public bool onValidGround { get; private set; }
        public bool onAnyGround { get; private set; }
        public float groundPositionY { get; private set; }
        public float slopeAngle { get; private set; }
        public Vector3 groundNormal { get; private set; }
        private RaycastHit groundHit;
        private CapsuleCollider capsuleCollider;

        private GroundProbeResult() {
            
        }
        public GroundProbeResult(CapsuleCollider collider)
        {
            capsuleCollider = collider;

            onValidGround = false;
            onAnyGround = false;
            groundPositionY = 0f;
            slopeAngle = 0f;
            groundNormal = Vector3.zero;
            groundHit = default;
        }
        public void UpdateData(RaycastHit groundhit, float maxSlope) {
            if (!groundhit.transform) {
                onAnyGround = false;
                onValidGround = false;
                return;
            }
            onAnyGround = true;
            groundNormal = groundhit.normal;
            slopeAngle = Vector3.Angle(Vector3.up, groundhit.normal);
            Vector3 targetBottomSphereCenter = groundhit.point + groundhit.normal * capsuleCollider.radius;
            groundPositionY = targetBottomSphereCenter.y - capsuleCollider.radius;
            onValidGround = slopeAngle <= maxSlope;
        }
    }
}