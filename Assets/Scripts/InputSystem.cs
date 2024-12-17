using System;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputSystem : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public Vector2 moveInput { get; private set; }

    public LayerMask interactableLayer;
    private IntercablesDetect intercablesDetect;
    public HintUIManager hintUIManager;

    public event Action<Vector2> ZoomInEvent;
    public event Action ZoomOutEvent;
    public event Action<float> AdjustZoomEvent;
    public event Action<Vector2> CursorMoveEvent;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        intercablesDetect = GetComponent<IntercablesDetect>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Interact.performed += OnInteract;
        inputActions.Player.Zoom.performed += HandleZoomIn;
        inputActions.Player.ZoomOut.performed += OnZoomOut;
        inputActions.Player.PanCamera.performed += OnMoveCursor;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Zoom.performed -= HandleZoomIn;
        inputActions.Player.ZoomOut.canceled -= OnZoomOut;
        inputActions.Player.PanCamera.performed -= OnMoveCursor;

    }

    private void Update()
    {
        float z = inputActions.Player.AdjustZoom.ReadValue<float>();
        if (z != 0)
        {
            AdjustZoomEvent?.Invoke(z);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.FreeRoam)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else
            moveInput = Vector2.zero;
    }

    private void HandleZoomIn(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.FreeRoam)
        {
            ZoomInEvent?.Invoke(inputActions.Player.Zoom.ReadValue<Vector2>());
        }
    }

    private void OnZoomOut(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            ZoomOutEvent?.Invoke();
        }
    }

    private void OnMoveCursor(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = inputActions.Player.PanCamera.ReadValue<Vector2>();
        if (mouseDelta.magnitude != 0)
        {
            CursorMoveEvent?.Invoke(mouseDelta);
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log(GameContext.Instance.state);
        switch ( GameContext.Instance.state)
            {
            case ContextState.FreeRoam:
                Interact();
                break;
            case ContextState.Tutorial:
                Interact();
                break;
            case ContextState.Zoomed:
                Interact();
                break;
        }
    }

    private void Interact()
    {
        GameObject closestInteractable = intercablesDetect.GetLastDetectedObject();
        if (closestInteractable != null)
        {
            Interactable interactable = closestInteractable.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

}
