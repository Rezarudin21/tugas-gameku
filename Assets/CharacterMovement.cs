using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float maxRunSpeed = 10f;
    public float runAcceleration = 2f;
    private float currentSpeed;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float runJumpBoost = 1.5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private int jumpCount = 0;
    public int maxJumpCount = 2;
    private bool jumpLocked = false;

    [Header("Grap & Climb")]
    public float hangingMoveSpeed = 3f;
    public float climbSpeed = 4f;
    private bool isHanging = false;
    private Transform grapTarget;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float runHoldTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        HandleMovementInput();
        CheckGrounded();

        if (!isHanging)
        {
            HandleRunning();
            HandleJump();
        }

        HandleGrap();

        if (isHanging)
        {
            HandleHangingMovement();
            HandleClimb();
        }
    }

    void FixedUpdate()
    {
        if (!isHanging)
        {
            Vector2 velocity = rb.velocity;
            velocity.x = movement.x * currentSpeed;
            rb.velocity = velocity;
        }
    }

    void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset jump count hanya jika player sudah double jump dan menyentuh tanah
        if (isGrounded && jumpLocked)
        {
            jumpCount = 0;
            jumpLocked = false;
        }
    }

    void HandleRunning()
    {
        if (Input.GetKey(KeyCode.LeftShift))
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Tambahkan pengecekan jumpLocked
            if (!jumpLocked && jumpCount < maxJumpCount)
            {
                Debug.Log("Jump ke-" + (jumpCount + 1));

                float finalJumpForce = jumpForce;

                if (currentSpeed > walkSpeed + 0.1f)
                {
                    finalJumpForce *= runJumpBoost;
                }

                rb.velocity = new Vector2(rb.velocity.x, finalJumpForce);
                jumpCount++;

                // Kunci jump jika sudah mencapai max
                if (jumpCount >= maxJumpCount)
                {
                    jumpLocked = true;
                }
            }
        }
    }

    void HandleGrap()
    {
        if (Input.GetKeyDown(KeyCode.E) && grapTarget != null)
        {
            Debug.Log("Grap aktif!");
            isHanging = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            transform.position = grapTarget.position;
        }
    }

    void HandleHangingMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(inputX * hangingMoveSpeed * Time.deltaTime, 0f, 0f);
    }

    void HandleClimb()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Climb!");
            isHanging = false;
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);

            // Reset jump count setelah climb
            jumpCount = 0;
            jumpLocked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GrapPoint"))
        {
            grapTarget = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GrapPoint"))
        {
            grapTarget = null;

            if (isHanging)
            {
                isHanging = false;
                rb.gravityScale = 1f;
            }
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
