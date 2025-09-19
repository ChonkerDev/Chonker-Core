using System;
using UnityEngine;

namespace Chonker2D.Sprite {
    public class SpriteParallaxEffect : MonoBehaviour {
        [SerializeField, Tooltip("Most likely the Main Camera")]
        private Transform TargetMovementReference;


        [SerializeField, Range(0, 1)] private float parallaxAmountX = .5f;
        [SerializeField, Range(0, 1)] private float parallaxAmountY = .5f;
        private Vector2 targetPosition;
        private float startXPos;
        private float startYPos;

        private void Awake() {
        }

        private void Start() {
            startXPos = TargetMovementReference.position.x;
            startYPos = transform.position.y;
        }

        private void Update() {
            float distanceToStartX = TargetMovementReference.transform.position.x - startXPos;
            float distanceToStartY = TargetMovementReference.transform.position.y - startYPos;
            float parallaxAmountFinalX = distanceToStartX * parallaxAmountX;
            float parallaxAmountFinalY = distanceToStartY * parallaxAmountY;
            targetPosition.x = startXPos + parallaxAmountFinalX;
            targetPosition.y = startYPos  + parallaxAmountFinalY;
            transform.position = targetPosition;
        }
    }
}