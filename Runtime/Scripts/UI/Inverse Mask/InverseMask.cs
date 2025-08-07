using System;
using UnityEngine;
using UnityEngine.UI;

namespace Chonker.Core.UI
{
    public class InverseMask : MonoBehaviour, IMaterialModifier
    {
        [SerializeField] private Image _image;
        public Image Image => _image;
        [SerializeField] private Material invertedStencilMat;
        [SerializeField] private Sprite _maskGraphic;

        public Material GetModifiedMaterial(Material baseMaterial) {
            return invertedStencilMat;
        }

        private void Reset() {
            if (!TryGetComponent<Image>(out var image)) {
                image = gameObject.AddComponent<Image>();
            }

            _image = image;
            _image.hideFlags = HideFlags.NotEditable;
            foreach (Graphic graphic in GetComponentsInChildren<Graphic>()) {
                if (graphic.gameObject == gameObject) continue;
                if (!graphic.TryGetComponent<InverseMaskContent>(out var maskContent)) {
                    graphic.gameObject.AddComponent<InverseMaskContent>();
                }
            }
        }
        
        private void OnDestroy() {
            foreach (Graphic graphic in GetComponentsInChildren<Graphic>()) {
                if (!graphic.TryGetComponent<InverseMaskContent>(out var maskContent)) {
                    DestroyImmediate(maskContent);
                }
            }
        }

        private void OnValidate() {
            _image.sprite = _maskGraphic;
            _image.material = invertedStencilMat;
        }
        
    }
}