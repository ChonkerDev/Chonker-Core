using System;
using UnityEngine;

namespace Chonker2D.Sprite {
    public class SpriteParallaxEffect : MonoBehaviour {
        [SerializeField, Tooltip("Most likely the Main Camera")]
        private Transform TargetMovementReference;

        [SerializeField] private bool _syncYPosition;

        [SerializeField, Range(0, 1)] private float parallaxAmountX = .5f;
        [SerializeField, Range(-1, 1)] private float parallaxAmountY = .5f;
        private Vector2 targetPosition;
        private float startXPos;
        private float startYPos;
        private float minParallax = .8f;
        private float maxParallax = 1;

        private void Awake() {
        }

        private void Start() {
            startXPos = TargetMovementReference.position.x;
            startYPos = TargetMovementReference.position.y;

            float yPos = transform.position.y;
            if (_syncYPosition && TargetMovementReference) {
                yPos = TargetMovementReference.position.y;
            }
            
            targetPosition = new Vector2(startXPos,  yPos);
        }

        private void Update() {
            float targetMovementReferenceXPos = TargetMovementReference.transform.position.x;
            float targetMovementReferenceYPos = TargetMovementReference.transform.position.y;
            float distanceToStartX = targetMovementReferenceXPos - startXPos;
            float distanceToStartY = targetMovementReferenceYPos - startYPos;
            float parallaxAmountFinalX = Mathf.Lerp(maxParallax, minParallax, parallaxAmountX);
            float parallaxAmountFinalY = Mathf.Lerp(maxParallax, minParallax, parallaxAmountY);
            targetPosition.x = startXPos + distanceToStartX * parallaxAmountFinalX;
            targetPosition.y = startYPos + distanceToStartY * parallaxAmountFinalY;
            if (_syncYPosition) {
                targetPosition.y = TargetMovementReference.position.y;
            }
            transform.position = targetPosition;
        }
    }
}