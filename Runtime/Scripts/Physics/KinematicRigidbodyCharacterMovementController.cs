using System;
using UnityEngine;

namespace Chonker.Core.Scripts.Physics
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public abstract class KinematicRigidbodyCharacterMovementController : MonoBehaviour
    {
        [Header("Capsule Collider")] 
        [SerializeField] private float _capsuleHeight = 2;

        [SerializeField] private float _capsuleRadius = .5f;

        [Header("Rigid Body")] 
        [SerializeField] private RigidbodyInterpolation _rigidbodyInterpolation;
        [SerializeField] private CollisionDetectionMode _collisionDetectionMode;

        [Header("Movement Settings")]
        public float skinWidth = 0.05f;
        public LayerMask collisionMask = ~0;
        [Range(1, 89)]public float slopeLimit = 50;

        [SerializeField, HideInInspector] private Rigidbody rb;
        [SerializeField, HideInInspector] private CapsuleCollider capsule;


        private Vector3 DesiredVelocity;

        public Vector3 RigidBodyVelocity => rb.linearVelocity;
        public GroundProbeResult GroundProbeResult { get; private set; }

        [Obsolete("Don't override Awake, use OnAwake instead", true)]
        void Awake() {
            GroundProbeResult = new GroundProbeResult(capsule);
            OnAwake();
        }

        protected virtual void OnAwake() {
        }

        [Obsolete("Don't override Fixed Update, use movement calculated methods", true)]
        void FixedUpdate() {
            float deltaTime = Time.fixedDeltaTime;
            BeforeMovementCalculated(deltaTime);

            UpdateVelocity(ref DesiredVelocity, deltaTime);

            if (GroundProbeResult.onAnyGround) {
                if (GroundProbeResult.onValidGround) // if on valid ground use the Desired Vel to influence, else either stand still or fall down the slope based on y vel
                {
                    Vector3 horizontalVel = new Vector3(DesiredVelocity.x, 0, DesiredVelocity.z);
                    float newY = -(horizontalVel.x * GroundProbeResult.groundNormal.x + horizontalVel.z * GroundProbeResult.groundNormal.z) / GroundProbeResult.groundNormal.y;
                    DesiredVelocity = new Vector3(horizontalVel.x, newY, horizontalVel.z);
                }
                else {
                    DesiredVelocity = Vector3.ProjectOnPlane(DesiredVelocity, GroundProbeResult.groundNormal);
                }
            }

            Vector3 finalMove = MoveAndCollide(DesiredVelocity);

            ProbeGround(finalMove);

            if (GroundProbeResult.onAnyGround) {
                finalMove.y = GroundProbeResult.groundPositionY;
                if (!GroundProbeResult.onValidGround) {
                    finalMove.y = Mathf.Min(rb.position.y, finalMove.y);
                }
            }

            rb.MovePosition(finalMove);

            AfterMovementCalculated(deltaTime);
        }


        private Vector3 MoveAndCollide(Vector3 deltaMove) {
            if (deltaMove == Vector3.zero)
                return rb.position;


            float remainingDistance = deltaMove.magnitude + skinWidth;
            Vector3 resolvedPosition = rb.position;
            int sweepIterations = 0;
            int maxIterations = 4;
            Vector3 sweepDirection = deltaMove.normalized;
            while(remainingDistance > 0 && sweepIterations < maxIterations) {
                sweepIterations++;
                Vector3 p1, p2;
                GetCapsulePoints(out p1, out p2, resolvedPosition);
                if (UnityEngine.Physics.CapsuleCast(p1, p2, capsule.radius,sweepDirection , out RaycastHit hit, remainingDistance
                        , collisionMask)) {
                    remainingDistance -= hit.distance;
                    remainingDistance = Mathf.Max(0, remainingDistance) + skinWidth;
                    sweepDirection = Vector3.ProjectOnPlane(deltaMove, hit.normal).normalized;
                    float surfaceAngle = Vector3.Angle(Vector3.up, hit.normal);
                    if (surfaceAngle > slopeLimit) {
                        sweepDirection.y = Mathf.Min(sweepDirection.y, 0);
                    }
                    resolvedPosition += sweepDirection * hit.distance;
                }
                else {
                    resolvedPosition += sweepDirection * remainingDistance;
                    remainingDistance = 0;
                }
            }

            return resolvedPosition;
        }

        private void ProbeGround(Vector3 atPosition) {
            Vector3 topCenter = atPosition + Vector3.up * (capsule.height - capsule.radius);

            float castDistance = capsule.height - capsule.radius * 2 +
                                 Mathf.Abs(rb.linearVelocity.y * Time.fixedDeltaTime) + 0.1f;
            Debug.DrawRay(topCenter, Vector3.down * castDistance, Color.red);
            if (UnityEngine.Physics.SphereCast(topCenter, capsule.radius, Vector3.down, out RaycastHit hit,
                    castDistance,
                    collisionMask)) {
                Debug.DrawRay(hit.point, hit.normal, Color.green);
            }

            GroundProbeResult.UpdateData(hit, slopeLimit);
        }

        private void GetCapsulePoints(out Vector3 p1, out Vector3 p2, Vector3 rbPos) {
            float height = Mathf.Max(capsule.height, capsule.radius * 2f);
            Vector3 center = rbPos + Vector3.up * capsule.center.y;
            Vector3 up = transform.up * (height / 2f - capsule.radius);

            p1 = center + up;
            p2 = center - up;
        }

        public abstract void UpdateVelocity(ref Vector3 DesiredVelocity, float deltaTime);

        protected virtual void BeforeMovementCalculated(float deltaTime) {
        }

        protected virtual void AfterMovementCalculated(float deltaTime) {
        }


        private void OnValidate() {
            if (!rb) {
                rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.hideFlags = HideFlags.NotEditable;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            rb.collisionDetectionMode = _collisionDetectionMode;
            rb.interpolation = _rigidbodyInterpolation;

            if (!capsule) {
                capsule = GetComponent<CapsuleCollider>();
                capsule.hideFlags = HideFlags.NotEditable;
            }

            capsule.center = Vector3.up * (_capsuleHeight / 2f);
            capsule.radius = _capsuleRadius;
            capsule.height = _capsuleHeight;
        }
    }
}