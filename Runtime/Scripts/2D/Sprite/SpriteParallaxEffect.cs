using System;
using UnityEngine;

namespace Chonker2D.Sprite {
    public class SpriteParallaxEffect : MonoBehaviour {
        [SerializeField, Tooltip("Most likely the Main Camera")]
        private Transform TargetMovementReference;
        
        [SerializeField] private float parallaxAmountX = 1;
        [SerializeField] private float parallaxAmountY = 1;
        [SerializeField] private float smoothingSpeed = 10;
        private Vector3 targetPosition;
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

            targetPosition.x = startXPos + parallaxAmountFinalX;
            targetPosition.y = startYPos + parallaxAmountFinalY;
            targetPosition.z = transform.position.z;

            Vector3 desiredPosition = new Vector3(
                startXPos + parallaxAmountFinalX,
                startYPos + parallaxAmountFinalY,
                transform.position.z);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothingSpeed);
        }
    }
}