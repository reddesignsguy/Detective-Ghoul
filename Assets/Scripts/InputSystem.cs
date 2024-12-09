using System;
using DS.ScriptableObjects;
using UnityEngine;
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
    public event Action<Vector2> CameraTargetEvent;

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
    }


    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Zoom.performed -= HandleZoomIn;
        inputActions.Player.ZoomOut.canceled += OnZoomOut;
    }

    private void Update()
    {
        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            Vector2 mouseDelta = inputActions.Player.PanCamera.ReadValue<Vector2>();
            if (mouseDelta.magnitude != 0)
            {
                CameraTargetEvent?.Invoke(mouseDelta);
            }
                

            float z = inputActions.Player.AdjustZoom.ReadValue<float>();
            if (z != 0)
            {
                AdjustZoomEvent?.Invoke(z);
            }

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
            GameContext.Instance.SetContextState(ContextState.Zoomed);

        }
    }

    private void OnZoomOut(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            ZoomOutEvent?.Invoke();
            GameContext.Instance.SetContextState(ContextState.FreeRoam);

        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (hintUIManager.IsEnabled())
        {
            hintUIManager.CloseUI();
        }
        else if (GameContext.Instance.state == ContextState.FreeRoam || GameContext.Instance.state == ContextState.SittingTutorial || GameContext.Instance.state == ContextState.StandingTutorial || GameContext.Instance.state == ContextState.Zoomed)
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
    }

}
