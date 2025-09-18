using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class NavigationUIMenu : MonoBehaviour {
    protected CanvasGroup canvasGroup;
    [SerializeField] protected Selectable defaultSelectable;
    [SerializeField] protected Selectable defaultSelectableOnDeactivate;
    [SerializeField] protected bool RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetected = true;
    private Coroutine coroutine;
    private RectTransform rectTransform;
    [SerializeField] private bool RestoreSelectionToDefaultIfCurrentIsNull;
    public UnityAction<GameObject, GameObject> OnCurrentSelectionChanged; 
    public RectTransform RectTransform => rectTransform;
    
    protected GameObject LastSelectedGameObjectForThisMenu;
    protected InputSystemUIInputModule uiInputModule;


    private static NavigationUIMenu currentFocusedMenu {
        get {
            if (navigationMenuStack.Count == 0) return null;
            return navigationMenuStack[^1];
        }
    }

    private static List<NavigationUIMenu> navigationMenuStack = new();

    [Obsolete("Use OnAwake instead", true)]
    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        OnAwake();
    }
    
    /*
     * if overriding, make sure to call base.OnEnable
     */
    protected void OnEnable() {
        uiInputModule = EventSystem.current?.GetComponent<InputSystemUIInputModule>();
        if (!uiInputModule) {
            Debug.LogError("Unable to find Event System in scene.");
            return;
        }

        if (RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetected) {
            uiInputModule.move.action.performed += RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetectedAction;
        }
    }

    protected void OnDisable() {
        if (RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetected) {
            uiInputModule.move.action.performed -= RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetectedAction;
        }
    }

    private void RestoreCurrentToDefaultIfCurrentIsNullAndMoveIsDetectedAction(InputAction.CallbackContext ctx) {
        if (currentFocusedMenu != this) return;
        if (EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(defaultSelectable.gameObject);
            StartCoroutine(EatMoveInput());
        }
    }

    private IEnumerator EatMoveInput() {
        uiInputModule.enabled = false;
        yield return null;
        uiInputModule.enabled = true;
    }

    protected virtual void OnAwake() {
    }

    [Obsolete("Use OnUpdate instead", true)]
    private void Update() {
        if (currentFocusedMenu == this) {
            if (LastSelectedGameObjectForThisMenu != EventSystem.current?.currentSelectedGameObject) {
                if (RestoreSelectionToDefaultIfCurrentIsNull && !EventSystem.current?.currentSelectedGameObject) {
                    EventSystem.current?.SetSelectedGameObject(defaultSelectable.gameObject);
                }
                OnCurrentSelectionChanged.Invoke(LastSelectedGameObjectForThisMenu, EventSystem.current?.currentSelectedGameObject);
                LastSelectedGameObjectForThisMenu = EventSystem.current?.currentSelectedGameObject;
            }
            processCurrentMenu();
        }

        OnUpdate();
    }

    protected virtual void processCurrentMenu() {
    }

    protected virtual void OnUpdate() {
    }

    public virtual void Activate() {
        if (defaultSelectable) {
            EventSystem.current?.SetSelectedGameObject(defaultSelectable.gameObject);
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        navigationMenuStack.Add(this);
    }

    public virtual void Deactivate() {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (defaultSelectableOnDeactivate) {
            EventSystem.current?.SetSelectedGameObject(defaultSelectableOnDeactivate.gameObject);
        }

        if (currentFocusedMenu == this) {
            navigationMenuStack.RemoveAt(navigationMenuStack.Count - 1);
        }
    }

    public void ClearCurrentInteractable() {
        EventSystem.current?.SetSelectedGameObject(null);
    }

    public void SetCanvasGroupAlpha(float alpha) {
        canvasGroup.alpha = alpha;
    }
}