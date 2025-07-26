using System;
using Chonker.Core.FirstPerson;
using UnityEngine;

namespace Chonker.Core
{
    public class ModelManager : MonoBehaviour
    {
        [SerializeField] private FirstPersonTorsoModelSource _modelSource;
        private FirstPersonTorsoModelSource modelSourceInstance;

        [SerializeField] private CameraControl _cameraControl;
        Vector3 hideAnchorOffset = Vector3.back;

        [SerializeField] private Vector3 torsoAnchorOffset;

        private void Start() {
            modelSourceInstance = Instantiate(_modelSource, transform);
            modelSourceInstance.transform.position = transform.position;
            torsoAnchorOffset = modelSourceInstance.torsoAnchorOffset;
        }

        private void LateUpdate() {
            modelSourceInstance.ModelHideAnchor.position = _cameraControl.transform.TransformPoint(hideAnchorOffset);
            modelSourceInstance.ModelHideAnchor.transform.rotation = _cameraControl.transform.rotation;
            modelSourceInstance.ModelCameraAnchor.transform.rotation = _cameraControl.transform.rotation;
            modelSourceInstance.ModelCameraAnchor.transform.position = _cameraControl.transform.TransformPoint(torsoAnchorOffset);
        }
    }
}