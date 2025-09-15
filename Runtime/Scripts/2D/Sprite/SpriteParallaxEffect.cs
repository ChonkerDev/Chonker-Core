using System;
using UnityEngine;

namespace Chonker2D.Sprite {
    public class SpriteParallaxEffect : MonoBehaviour {
        [SerializeField, Tooltip("Most likely the Main Camera")]
        private Transform TargetMovementReference;

        [SerializeField, Range(0, 1)] private float parallaxAmount = .5f;
        private Vector2 targetPosition;
        private float startXPos;
        private float minParallax = .8f;
        private float maxParallax = 1;

        private void Awake() {
        }

        private void Start() {
            targetPosition = transform.position;
            if (TargetMovementReference) {
                startXPos = TargetMovementReference.position.x;
            }
            else {
                startXPos = transform.position.x;
            }
            
            targetPosition = new Vector2(startXPos,  transform.position.y);
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