using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public Vector2 moveInput { get; private set; }
    private float interactRange = 1f;

    public LayerMask interactableLayer;
    private IntercablesDetect intercablesDetect;

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
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Interact.performed -= OnInteract;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnInteract(InputAction.CallbackContext context)
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
