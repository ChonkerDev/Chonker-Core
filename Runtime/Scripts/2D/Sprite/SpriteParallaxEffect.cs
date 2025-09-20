using System;
using UnityEngine;

namespace Chonker2D.Sprite {
    public class SpriteParallaxEffect : MonoBehaviour {
        [SerializeField, Tooltip("Most likely the Main Camera")]
        private Transform TargetMovementReference;
        
        [SerializeField] private float parallaxAmountX = 0;
        [SerializeField] private float parallaxAmountY = 0;
        private Vector2 targetPosition;
        private float startXPos;
        private float startYPos;

        private void Awake() {
        }

        private void Start() {
            startXPos = transform.position.x;
            startYPos = transform.position.y;
        }

        private void LateUpdate() {
            float distanceToStartX = TargetMovementReference.transform.position.x - startXPos;
            float distanceToStartY = TargetMovementReference.transform.position.y - startYPos;
            float parallaxAmountFinalX = distanceToStartX * parallaxAmountX;
            float parallaxAmountFinalY = distanceToStartY * parallaxAmountY;
            targetPosition.x = TargetMovementReference.transform.position.x + parallaxAmountFinalX;
            targetPosition.y = TargetMovementReference.transform.position.y + parallaxAmountFinalY;
            transform.position = targetPosition;
        }
    }
}