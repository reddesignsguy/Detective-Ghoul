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
    private Animator animator ;

    public AudioSource footsteps;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {

        EventsManager.instance.onSetMovement += SetMovement;
    }

    private void OnDisable()
    {
        EventsManager.instance.onSetMovement -= SetMovement;

    }
    private void Update()
    {
         if (isMovementEnabled)
        {
        Vector2 moveInput = inputSystem.moveInput;
        Vector2 moveX = new Vector3(moveInput.x, 0);
        rb.velocity = moveX * moveSpeed;
        if (moveInput.x != 0)
            {
            sr.flipX = moveInput.x < 0;
            animator.Play("Mouvement");
                if (!footsteps.isPlaying)
                {
                    footsteps.Play();
                }
            }
            
            else
            {
            animator.Play("Idle");
            footsteps.Stop();
            }
            
            
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

    private void SetMovement(bool on)
    {
        isMovementEnabled = on;

    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }
}
