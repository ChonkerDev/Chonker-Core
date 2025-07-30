using System;
using Chonker.Core.FirstPerson;
using UnityEngine;

namespace Chonker.Core
{
    [DefaultExecutionOrder(-100)]
    public class ModelManager : MonoBehaviour
    {
        [SerializeField] private FirstPersonTorsoModelSource _modelSource;
        public FirstPersonTorsoModelSource modelSourceInstance { get; private set; }

        public CameraControl CameraControl;
        Vector3 hideAnchorOffset = Vector3.back;

        [SerializeField] private Vector3 torsoAnchorOffset;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public FirstPersonTorsoAnimationController FirstPersonTorsoAnimationController;
        private void Awake() {
            modelSourceInstance = Instantiate(_modelSource, transform);
            modelSourceInstance.transform.position = transform.position;
            torsoAnchorOffset = modelSourceInstance.torsoAnchorOffset;
            modelSourceInstance.Animator.runtimeAnimatorController = _animatorController;
            FirstPersonTorsoAnimationController.Initialize(modelSourceInstance.Animator);
        }

        private void LateUpdate() {
            modelSourceInstance.ModelHideAnchor.position = CameraControl.transform.TransformPoint(hideAnchorOffset);
            modelSourceInstance.ModelHideAnchor.transform.rotation = CameraControl.transform.rotation;
            modelSourceInstance.ModelCameraAnchor.transform.rotation = CameraControl.transform.rotation;
            modelSourceInstance.ModelCameraAnchor.transform.position = CameraControl.transform.TransformPoint(torsoAnchorOffset);
        }
    }
}