using Chonker.Core.Scripts.Physics;
using UnityEditor;
using UnityEngine;

namespace Chonker.Core.Editor
{
    [CustomEditor(typeof(KinematicRigidbodyCharacterMovementController))]
    internal class KinematicRigidbodyCharacterEditor : UnityEditor.Editor
    {
        private CapsuleCollider _capsuleCollider;

        void OnEnable() {
            var character = (KinematicRigidbodyCharacterMovementController)target;
            _capsuleCollider = character.GetComponent<CapsuleCollider>();
            _capsuleCollider.hideFlags = HideFlags.NotEditable;
        }

        void OnDisable() {
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            EditorGUILayout.Space();
        }
    }
}