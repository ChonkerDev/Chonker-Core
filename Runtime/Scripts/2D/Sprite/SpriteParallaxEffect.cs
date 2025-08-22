using System;
using UnityEngine;

namespace Chonker2D.Sprite
{
    public class SpriteParallaxEffect : MonoBehaviour
    {
        [SerializeField] private bool AutoSetTargetMovementReferenceToMainCamera;
        [SerializeField, Tooltip("Most likely the Main Camera")] private Transform TargetMovementReference;
        [SerializeField, Range(0, 1)] private float parallaxAmount = .5f;
        private Vector2 targetPosition;
        private float startXPos;
        private float minParallax = .8f;
        private float maxParallax = 1;
        private void Awake() {
            if (AutoSetTargetMovementReferenceToMainCamera) {
                TargetMovementReference = Camera.main.transform;
            }
            targetPosition = transform.position;
            startXPos = TargetMovementReference.transform.position.x;
        }

        private void Update() {
            float targetMovementReferenceXPos = TargetMovementReference.transform.position.x;
            float distanceToStart = targetMovementReferenceXPos - startXPos;
            float parallaxAmountFinal = Mathf.Lerp(maxParallax, minParallax, parallaxAmount);
            targetPosition.x = startXPos + distanceToStart * parallaxAmountFinal;
            transform.position = targetPosition;
        }
    }
}