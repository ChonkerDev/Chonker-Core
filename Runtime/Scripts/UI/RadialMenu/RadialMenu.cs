using System;
using System.Collections;
using Chonker.Core.Scripts.Physics;
using Chonker.Core.Tween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Chonker.Core.UI
{
    public class RadialMenu : MonoBehaviour
    {
        public const int maxNumWedges = 10;
        public const int minNumWedges = 2;

        [SerializeField, Range(minNumWedges, maxNumWedges)]
        private int _numWedges = 2;

        [SerializeField] private Transform wedgeContainer;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private CircleCollider2D _CenterBlockerCollider;
        [SerializeField] private RadialMenuWedge wedgeTemplate;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private RadialMenuWedge[] wedges;
        [SerializeField, Range(100, 2000)] private int _radius;
        [SerializeField, Range(0, 1)] private float _iconOffset;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField, Range(0, 180)] private int _wedgeRotationOffset;

        [SerializeField] private Color _wedgeColor = Color.gray;
        [SerializeField] private Color _backgroundColor = Color.white;
        [SerializeField] private bool _updateWedgeOnHover = true;
        [SerializeField, Range(1, 2)] private float _onHoverScale = 1.1f;
        [SerializeField] private Color _onHoverColor = Color.white;
        [SerializeField, Range(.01f, 1)] private float _hoverTransitionTime = 0.2f;
        [SerializeField] private bool _activationTransition = true;

        [SerializeField] private AudioClip _onHoverEnterWedgeSoundClip;
        [SerializeField] private AudioClip _onSubmitWedgeSoundClip;

        public UnityEvent<RadialMenuWedge> OnWedgeHover;
        public UnityEvent<RadialMenuWedge> OnWedgeUnhover;
        public UnityEvent<RadialMenuWedge> OnWedgeSubmitted;

        private LayerMask UIMask;
        [SerializeField] private Camera uiCamera;
        Collider2D[] wedgeProbeResults = new Collider2D[1];

        private CanvasGroup canvasgroup;
        public bool isOn { get; private set; }
        public bool TrackMouse = true;

        private RadialMenuWedge _currentWedge;

        public RadialMenuWedge CurrentWedge {
            get => _currentWedge;
            set {
                if (_currentWedge) {
                    OnWedgeUnhover.Invoke(_currentWedge);
                    _currentWedge.OnUnHover();
                }

                _currentWedge = value;
                if (_currentWedge) {
                    _audioSource.PlayOneShot(_onHoverEnterWedgeSoundClip);
                    OnWedgeHover.Invoke(_currentWedge);
                    _currentWedge.OnHover();
                }
            }
        }

        private void Awake() {
            wedgeTemplate.gameObject.SetActive(false);
            canvasgroup = GetComponent<CanvasGroup>();
        }

        private IEnumerator Start() {
            yield return new WaitForEndOfFrame();
            TurnOff();
        }

        public void TurnOn() {
            isOn = true;
            gameObject.SetActive(isOn);
            if (_activationTransition) {
                StartCoroutine(TweenCoroutines.RunTaper(.2f, alpha => {
                        transform.localScale = Vector3.one * alpha;
                        canvasgroup.alpha = alpha;
                    },
                    null, EaseType.SmoothStep));
            }
            else {
                canvasgroup.alpha = 1;
            }
        }

        public void TurnOff() {
            isOn = false;
            canvasgroup.alpha = 0;
            gameObject.SetActive(isOn);
        }

        private void Update() {
            if (TrackMouse) {
                probeForWedges();

                if (Input.GetMouseButtonDown(0)) {
                    SubmitCurrentWedge();
                }
            }
        }

        public void SubmitCurrentWedge() {
            _audioSource.PlayOneShot(_onSubmitWedgeSoundClip);
            OnWedgeSubmitted?.Invoke(CurrentWedge);
        }

        private void probeForWedges() {
            Debug.Log(CurrentWedge?.gameObject.GetInstanceID());
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1;
            Vector2 mouseWorldPos = uiCamera.ScreenToWorldPoint(mousePos);

            ContactFilter2D filter = new ContactFilter2D {
                useLayerMask = true,
                layerMask = PhysicsLayerRegistry.UIMask,
                useTriggers = true
            };

            int hitCount = Physics2D.OverlapPoint(mouseWorldPos, filter, wedgeProbeResults);

            if (hitCount > 0 && wedgeProbeResults[0].CompareTag("UI_Wedge")) {
                Collider2D hitCollider = wedgeProbeResults[0];
                if (!CurrentWedge) {
                    if (hitCollider.TryGetComponent<RadialMenuWedge>(out var foundWedge)) {
                        CurrentWedge = foundWedge;
                    }

                    return;
                }

                if (hitCollider && hitCollider.transform != CurrentWedge.transform) {
                    if (hitCollider.TryGetComponent<RadialMenuWedge>(out var foundWedge)) {
                        CurrentWedge = foundWedge;
                    }

                    return;
                }
            }
            else {
                CurrentWedge = null;
            }
        }

        public void rebuildMenu() {
            int childCount = wedgeContainer.childCount;
            for (int i = 0; i < childCount; i++) {
                DestroyImmediate(wedgeContainer.GetChild(0).gameObject);
            }

            wedges = new RadialMenuWedge[_numWedges];
            for (var i = 0; i < wedges.Length; i++) {
                RadialMenuWedge wedge = Instantiate(wedgeTemplate, wedgeContainer);
                wedge.gameObject.SetActive(true);
                wedges[i] = wedge;
                float fillAmount = 1f / wedges.Length;
                float rotation = (float)i / wedges.Length * 360f;
                wedge.setWedgeTransformData(fillAmount, rotation, _radius, wedges.Length, _iconOffset,
                    _wedgeRotationOffset);
                wedge.setParentMenuReference(this);
            }

            _CenterBlockerCollider.radius = _radius / 4f;
            refreshSize();
            refreshVisualData();
        }

        public void refreshVisualData() {
            _backgroundImage.color = _backgroundColor;
            foreach (var radialMenuWedge in wedges) {
                radialMenuWedge.setWedgeVisualData(_updateWedgeOnHover, _wedgeColor, _onHoverScale, _onHoverColor,
                    _hoverTransitionTime);
            }
        }

        private void refreshSize() {
            rectTransform.sizeDelta = new Vector2(_radius, _radius);
        }

        public void SetWedgeBasedOnDirection(Vector2 direction) {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;
            float wedgeAngleSize = 360f / wedges.Length;
            int index = Mathf.RoundToInt(angle / wedgeAngleSize) % wedges.Length;
            CurrentWedge = wedges[index];
        }

        private void OnValidate() {
            if (!rectTransform) {
                rectTransform = GetComponent<RectTransform>();
            }
        }
    }
}