using DS.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public Vector2 moveInput { get; private set; }
    private float interactRange = 1f;

    public LayerMask interactableLayer;
    private IntercablesDetect intercablesDetect;
    public HintUIManager hintUIManager;

    private bool mouvementEnabled = false;
    private bool detectEnabled = true;


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
        DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
        DialogueEvents.instance.onExitedOptions += EnableMobility;


    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Interact.performed -= OnInteract;
        DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
        DialogueEvents.instance.onExitedOptions -= EnableMobility;


    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (mouvementEnabled)
            moveInput = context.ReadValue<Vector2>();
        else
            moveInput = Vector2.zero;
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
        else if (detectEnabled)
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

    private void HandleDialogueStarted(DSDialogueSO sO)
    {
        if (sO == null)
        {
            EnableMobility();
        }
        else
        {
            DisableMobility();
        }
    }

    private void EnableMobility()
    {
        detectEnabled = true;
        mouvementEnabled = true;
    }

    private void DisableMobility()
    {
        detectEnabled = false;
        mouvementEnabled = false;
    }
}
