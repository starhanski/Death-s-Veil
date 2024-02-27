using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100.0f;
    private float knockbackStartTime;

    [SerializeField] private float knockbackDuration;
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float jumpFoce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float airDragMultiplier = 0.95f;
    public float jumpHeightMultiplier = 0.5f;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool canMove;
    private bool canFlip;
    private bool isGrounded;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool hasWallJump;
    private bool isDashing;
    private bool knockback;

    [SerializeField] private Vector2 knockbackSpeed;
    private Rigidbody2D rb;
    private Animator anim;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsGround;

    public Vector2 wallJumpDirecton;

    private int lastWallJumpDirection;
    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    public int amountOfJumps = 1;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallJumpDirecton.Normalize();
    }

    void Update()
    {
        CheckInput();
        CheckJump();
        CheckMovementDirection();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckDash();
        CheckKnockback();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {


        ApplyMovement();
        CheckSurroundings();
    }


    private void CheckSurroundings()//
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    }

    private void CheckIfCanJump()
    {

        if (isGrounded && rb.velocity.y <= 0.4)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
        if (Math.Abs(rb.velocity.x) >= 0.3f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
    
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NoramlJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;
                turnTimer = turnTimerSet;
            }
        }
        if (!canMove)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }
        if (checkJumpMultiplier && !Input.GetButton("Jump")) //Jump Height
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
        }
        if (Input.GetButtonDown("Dash"))
        {
            if (Time.time >= (lastDash + dashCoolDown))
            {
                AttemptToDash();

            }
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {

            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NoramlJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
        if (hasWallJump && movementInputDirection == -lastWallJumpDirection) //fixing second jump from one wall
        {
            rb.velocity = new Vector2(rb.velocity.x, -15f);

        }
        else if (wallJumpTimer <= 0)
        {
            hasWallJump = false;
        }
        else
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                if (movementInputDirection != 0)
                {
                    facingDirection = (int)movementInputDirection;

                }
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0f);
                dashTimeLeft -= Time.deltaTime;
                if (Math.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }
    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    public bool GetDashStatus()
    {
        return isDashing;
    }
    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }
    private void NoramlJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpFoce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }
    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirecton.x * movementInputDirection, wallJumpForce * wallJumpDirecton.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJump = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }
    private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);

        }


        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }
    public void DisableFlip()
    {
        canFlip = false;

    }
    public void EnableFlip()
    {
        canFlip = true;


    }



    private void Flip()
    {
        facingDirection *= -1;
        if (!isWallSliding && canFlip && !knockback)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));

    }
}
