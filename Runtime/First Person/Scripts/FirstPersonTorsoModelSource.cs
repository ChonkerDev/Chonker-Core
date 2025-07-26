using System;
using Chonker.Core.Scripts.Physics;
using UnityEngine;

namespace Chonker.Core
{
    public class FirstPersonTorsoModelSource : MonoBehaviour
    {
        public Transform ModelCameraAnchor;
        [Tooltip("Most likely \"root\"")]
        public Transform ModelHideAnchor;

        public Vector3 torsoAnchorOffset;
        private void Awake() {
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
                skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                gameObject.layer = PhysicsLayerRegistry.LocalPlayerLayerIndex;
            }
        }
    }
}
