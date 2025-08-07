using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chonker.Core.UI
{
    public class RadialMenuWedge : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _icon;
        [SerializeField] private PolygonCollider2D _polygon;
        [SerializeField] private bool _updateDisplayOnHover;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private float _hoverScale;
        [SerializeField, HideInInspector] private RadialMenu parentRadialMenu;
        [SerializeField] private float _hoverTransitionTime = 0.2f;
        private float currentTransitionTimer = 0f;

        private void Awake() {
            _image = GetComponent<Image>();
        }
        
        public void setParentMenuReference(RadialMenu radialMenu) {
            parentRadialMenu = radialMenu;
        }

        public void OnHover() {
            if (_updateDisplayOnHover) {
                StopAllCoroutines();
                StartCoroutine(OnHoverI());
            }
        }

        public void OnUnHover() {
            if (_updateDisplayOnHover) {
                StopAllCoroutines();
                StartCoroutine(OnUnHoverI());
            }

        }

        public void setWedgeTransformData(float fillSize, float rotation, float radius, int numWedges, float iconOffset, float wedgeRotationOffset) {
            transform.localPosition = Vector3.zero;
            _image.fillAmount = fillSize;
            transform.rotation = Quaternion.Euler(0, 0, rotation + wedgeRotationOffset);
            _icon.transform.rotation = Quaternion.Euler(0, 0, 0);
            float iconDistance = radius * Mathf.Lerp(1/3f, 1f, iconOffset);
            Vector3 rotated = Quaternion.AngleAxis(360f / numWedges / 2, Vector3.forward) * Vector3.right * iconDistance;
            _icon.transform.localPosition = rotated;
            float iconSizeDampen = 6f;
            _icon.GetComponent<RectTransform>().sizeDelta = Vector2.one * radius / iconSizeDampen;
            _polygon.points = buildPolygonCollider(radius, numWedges);
        }

        public void setWedgeVisualData(bool updateDisplayOnHover, Color wedgeColor, float hoverScale, Color hoverColor, float hoverTransitionTime) {
            _updateDisplayOnHover = updateDisplayOnHover;
            _defaultColor = wedgeColor;
            _image.color = wedgeColor;
            _hoverColor = hoverColor;
            _hoverScale = hoverScale;
            _hoverTransitionTime = hoverTransitionTime;
        }

        private Vector2[] buildPolygonCollider(float radius, int numWedges) {
            List<Vector2> points = new();
            int maxNumWedges = RadialMenu.maxNumWedges;
            int numIterations = Mathf.Max(maxNumWedges - numWedges, 3);
            float totalDegrees = 360f / numWedges;
            float degreesPerIteration = totalDegrees / numIterations;
            Vector2 center = Vector2.zero;
            points.Add(center);
            float pointDistance = radius / 2f;
            for (int i = 0; i <= numIterations; i++) {
                float degrees = degreesPerIteration * i;
                float x = Mathf.Cos(degrees * Mathf.Deg2Rad) * pointDistance;
                float y = Mathf.Sin(degrees * Mathf.Deg2Rad) * pointDistance;
                points.Add(new Vector2(x, y));
            }

            return points.ToArray();
        }

        private IEnumerator OnHoverI() {
            while (currentTransitionTimer < 1) {
                float deltaTime = Time.deltaTime / _hoverTransitionTime;
                currentTransitionTimer += deltaTime;
                setVisuals(currentTransitionTimer);
                yield return null;
            }
        }

        private IEnumerator OnUnHoverI() {
            while (currentTransitionTimer > 0) {
                float deltaTime = Time.deltaTime / _hoverTransitionTime;
                currentTransitionTimer -= deltaTime;
                setVisuals(currentTransitionTimer);
                yield return null;
            }
        }

        private void setVisuals(float alpha) {
            transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * _hoverScale, alpha);
            _image.color = Color.Lerp(_defaultColor, _hoverColor, alpha);
        }

        private void OnValidate() {
            if (!_image) {
                _image = GetComponent<Image>();
            }

            if (!_polygon) {
                _polygon = GetComponent<PolygonCollider2D>();
            }
        }
    }
}