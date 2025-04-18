using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float maxRunSpeed = 10f;
    public float runAcceleration = 2f;
    public float jumpForce = 7f;
    public float runJumpBoost = 1.5f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float currentSpeed;
    private float runHoldTime;
    private float moveInput;
    private bool facingRight = true;
    private bool isGrounded;

    private CharacterAnimator characterAnimator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<CharacterAnimator>();

        rb.freezeRotation = true;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        HandleRunning();
        HandleJump();
        HandleFlip();

        // Kirim status ke animator
        if (characterAnimator != null)
            characterAnimator.UpdateAnimationState(moveInput, isGrounded, Input.GetKey(KeyCode.LeftShift));
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }

    void HandleRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift) && moveInput != 0)
        {
            runHoldTime += Time.deltaTime;
            currentSpeed = Mathf.Min(walkSpeed + runHoldTime * runAcceleration, maxRunSpeed);
        }
        else
        {
            runHoldTime = 0f;
            currentSpeed = walkSpeed;
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float jump = (currentSpeed > walkSpeed + 0.1f) ? jumpForce * runJumpBoost : jumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
    }

    void HandleFlip()
    {
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
