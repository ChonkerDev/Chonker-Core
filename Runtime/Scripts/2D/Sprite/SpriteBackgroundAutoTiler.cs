using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chonker2D.Sprite
{
    public class SpriteBackgroundAutoTiler : MonoBehaviour
    {
        SpriteRenderer[] spriteRenderers;
        private float baseOffset;
        private int numSprites;
        private Camera mainCamera;
        private float[] defaultXPositions;
        private float widths;

        private int currentCameraViewIndex = 0;

        private void Awake() {
            List<SpriteRenderer> spriteRendererList = new List<SpriteRenderer>();
            for (int i = 0; i < transform.childCount; i++) {
                SpriteRenderer sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
                spriteRendererList.Add(sr);
            }

            spriteRenderers = spriteRendererList.ToArray();
            widths = spriteRenderers[0].bounds.size.x;
            mainCamera = Camera.main;
            numSprites = spriteRenderers.Length;
            baseOffset = -widths * (numSprites - 1) / 2;
            currentCameraViewIndex = calculateCurrentCameraViewCycleIndex();
            calculateBasePositions();

        }

        private void Start() {
            defaultXPositions = new float[spriteRenderers.Length];
            for (var i = 0; i < spriteRenderers.Length; i++) {
                defaultXPositions[i] = spriteRenderers[i].transform.localPosition.x;
            }
        }

        private void Update() {
            int oldCameraViewIndex = currentCameraViewIndex;
            currentCameraViewIndex = calculateCurrentCameraViewCycleIndex();
            if (oldCameraViewIndex != currentCameraViewIndex) {
                Debug.Log(currentCameraViewIndex);
                calculateBasePositions();
            }
        }

        private void calculateBasePositions() {
            int cycleIndex = currentCameraViewIndex % numSprites;
            for (var i = 0; i < spriteRenderers.Length; i++) {
                Vector2 position = Vector2.zero;
                int slotIndex = (i - cycleIndex + numSprites) % numSprites;
                float finalPosition = (currentCameraViewIndex * widths) + (slotIndex * widths) +
                                      baseOffset;
                position.x = finalPosition;
                spriteRenderers[i].transform.localPosition = position;
            }
        }


        private int calculateCurrentCameraViewCycleIndex() {
            float position = mainCamera.transform.position.x;
            float distanceToStartingPosition = position - transform.position.x;
            return Mathf.RoundToInt(distanceToStartingPosition / widths);
        }
    }
}