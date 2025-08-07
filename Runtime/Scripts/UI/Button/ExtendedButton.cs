using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chonker.Core
{
    [RequireComponent(typeof(Button))]
    public class ExtendedButton : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Button _underlyingButton;
        private Image image => _underlyingButton.image;
        public Button UnderlyingButton => _underlyingButton;
        public Button.ButtonClickedEvent onClick => _underlyingButton.onClick;

        public void setOverrideSprite(Sprite sprite) {
            image.overrideSprite = sprite;
        }

        public void SimulateClick() {
            Debug.Log("Simulated Click");
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var pev = new PointerEventData(eventSystem) {
                pointerId = -1,
                position = Vector2.zero,
            };


            ExecuteEvents.Execute(_underlyingButton.gameObject, pev, ExecuteEvents.pointerEnterHandler);

            ExecuteEvents.Execute(_underlyingButton.gameObject, pev, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(_underlyingButton.gameObject, pev,
                ExecuteEvents.pointerClickHandler); // <-- This is crucial
            ExecuteEvents.Execute(_underlyingButton.gameObject, pev, ExecuteEvents.pointerUpHandler);

            ExecuteEvents.Execute(_underlyingButton.gameObject, pev, ExecuteEvents.pointerExitHandler);
        }

        private void OnValidate() {
            if (!_underlyingButton) {
                _underlyingButton = GetComponent<Button>();
            }
        }
    }
}