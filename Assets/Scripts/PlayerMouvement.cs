using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    [SerializeField] public InputSystem inputSystem;
    private Vector2 moveInput;
    [SerializeField]public float moveSpeed = 5f;
    private Rigidbody rb;
    private SpriteRenderer sr;
    [SerializeField]public float raycastDistance = 1.5f;
    [SerializeField]public LayerMask groundLayer;
    private bool isMovementEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {

        EventsManager.instance.onStartKeySelection += DisableMovement;
        EventsManager.instance.onEndKeySelection += EnableMovement;
    }

    private void OnDisable()
    {
        EventsManager.instance.onStartKeySelection -= DisableMovement;
        EventsManager.instance.onEndKeySelection -= EnableMovement;
    }
    private void Update()
    {
         if (isMovementEnabled)
        {
        Vector2 moveInput = inputSystem.moveInput;
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
