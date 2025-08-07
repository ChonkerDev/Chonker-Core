using System;
using UnityEngine;
using UnityEngine.UI;

namespace Chonker.Core.UI
{
    [ExecuteInEditMode]
    public class InverseMaskContent : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Material _inverseMaskContent;
        [SerializeField, HideInInspector] private Material oldMaterial;

        private void Reset() {
            Graphic image = GetComponent<Graphic>();
            if (image) {
                oldMaterial = image.material;
                image.material = _inverseMaskContent;
            }
        }

        private void OnDestroy() {
            Graphic image = GetComponent<Graphic>();
            if (image) {
                image.material = oldMaterial;
            }
        }
    }
}
