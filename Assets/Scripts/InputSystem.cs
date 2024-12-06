using System;
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
    private Vector2 mouseDelta = Vector2.zero;
    private bool detectEnabled;
    private bool mouvementEnabled;

    // Smooth panning
    private Vector3 freeRoamCameraReturnPos;
    private Vector3 targetPosition;
    public float zoomSetting1 = 15f;
    public float sensitivity = 0.5f;
    public float lerpSpeed = 5f; // Adjust for how fast it reaches the target
    public float zoomSensitivity = 5f;

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
        inputActions.Player.Zoom.performed += OnZoomIn;
        inputActions.Player.Zoom.canceled += OnZoomOut;
        DialogueEvents.instance.onDialogueStarted += HandleDialogueStarted;
        DialogueEvents.instance.onExitedOptions += EnableMobility;
    }


    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Interact.performed -= OnInteract;
        inputActions.Player.Zoom.performed -= OnZoomIn;
        inputActions.Player.Zoom.canceled -= OnZoomOut;
        DialogueEvents.instance.onDialogueStarted -= HandleDialogueStarted;
        DialogueEvents.instance.onExitedOptions -= EnableMobility;


    }

    private void FixedUpdate()
    {
        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            // Update the target position based on mouse delta
            Vector3 cameraDisplacement = new Vector3(mouseDelta.x, mouseDelta.y, 0) * sensitivity;
            targetPosition += cameraDisplacement;

            // Smoothly move the camera towards the target position with easing
            Camera.main.transform.position = SmoothLerp(Camera.main.transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }
    private void Update()
    {
        mouseDelta = inputActions.Player.PanCamera.ReadValue<Vector2>();

        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            float z = inputActions.Player.AdjustZoom.ReadValue<float>();
            if (z > 0)
            {
                Camera.main.fieldOfView += zoomSensitivity;

            }
            else if (z < 0)
            {
                Camera.main.fieldOfView -= zoomSensitivity;
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

    private void OnZoomIn(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.FreeRoam)
        {
            freeRoamCameraReturnPos = Camera.main.transform.position;
            Vector2 mousePos = inputActions.Player.Zoom.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePos); // Create a ray from the camera to the mouse position
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = new Vector3(hit.point.x, hit.point.y, Camera.main.transform.position.z);
                Camera.main.transform.position = targetPosition;
            }
            else
            {
                targetPosition = Camera.main.transform.position;
            }
           

            Camera.main.fieldOfView = zoomSetting1;
            
            GameContext.Instance.SetContextState(ContextState.Zoomed);
            
        }
    }

    private void OnZoomOut(InputAction.CallbackContext context)
    {
        if (GameContext.Instance.state == ContextState.Zoomed)
        {
            Camera.main.fieldOfView = 38;
            GameContext.Instance.SetContextState(ContextState.FreeRoam);
            Camera.main.transform.position = freeRoamCameraReturnPos;
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
        else if (GameContext.Instance.state == ContextState.FreeRoam || GameContext.Instance.state == ContextState.SittingTutorial || GameContext.Instance.state == ContextState.StandingTutorial)
        {
            Debug.Log("Interacting with game object");
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

    // Custom Lerp with ease-in/ease-out
    private Vector3 SmoothLerp(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t); // Ensure t is between 0 and 1
        t = t * t * (3f - 2f * t); // Smoothstep easing
        return Vector3.Lerp(start, end, t);
    }
}
