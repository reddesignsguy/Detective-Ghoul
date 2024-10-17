using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private SpriteRenderer sr;
    public float raycastDistance = 1.5f;
    public LayerMask groundLayer;
    private bool isMovementEnabled = true;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        EventsManager.instance.onStartKeySelection += DisableMovement;
        EventsManager.instance.onEndKeySelection += EnableMovement;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        EventsManager.instance.onStartKeySelection -= DisableMovement;
        EventsManager.instance.onEndKeySelection -= EnableMovement;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
         if (isMovementEnabled)
        {
        Vector2 moveX = new Vector3(moveInput.x, 0);
        rb.velocity = moveX * moveSpeed;
        if (moveInput.x != 0) sr.flipX = moveInput.x < 0;
        }
    }

    private void FixedUpdate()
    {
        if (isMovementEnabled)
        {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 0.5f;

        if (Physics.Raycast(castPos, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = rb.position;
                movePos.y = hit.point.y + raycastDistance;
                rb.MovePosition(movePos);
            }
        }
        }
    }

     private void DisableMovement(LockedToggleable toggleable)
    {
        isMovementEnabled = false;
    }

    private void EnableMovement(LockedToggleable toggleable)
    {
        isMovementEnabled = true;
    }
}
