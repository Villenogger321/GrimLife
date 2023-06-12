using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool hasStartedJumpAnimation;

    Vector2 movementInput = new Vector2();
    Vector3 moveDirection;
    float xRotation;
    Rigidbody rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        readyToJump = true;
        hasStartedJumpAnimation = false;
    }

    private void Update()
    {
        grounded = CheckGrounded();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        bool isMoving = rb.velocity.magnitude > 0.1f;
        bool isJumping = !readyToJump;

        animator.SetBool("IsRunning", isMoving);
        animator.SetBool("IsJumping", isJumping);
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);

        if (!grounded && !hasStartedJumpAnimation)
        {
            animator.SetBool("IsJumping", true);
            hasStartedJumpAnimation = true;
        }
        if (grounded)
        {
            animator.SetBool("IsJumping", false);
            hasStartedJumpAnimation = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void OnJump()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void OnMove(InputValue _value)
    {
        movementInput = _value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = transform.forward * movementInput.y + transform.right * movementInput.x;
        transform.LookAt(transform.GetChild(1).transform);

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    bool CheckGrounded()
    {
        // Raycast down from the player's position to check if it's grounded
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        return isGrounded;
    }
}
