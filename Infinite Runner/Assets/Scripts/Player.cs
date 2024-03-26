using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] int amountOfJumps = 2;
    [Tooltip("All the jumps after the first jump have this modifier, ie, if we want the other jumps to be less we make this a value between 0-1")] 
    [SerializeField] float secondaryJumpModifiers = .85f;
    [SerializeField] float wallSlideSpeed = 2f;
    [SerializeField] Transform groundCheck, wallCheck;
    [SerializeField] LayerMask groundLayers, wallLayers;

    [Header("Wall jump vars")]
    [Tooltip("if this is true, the players horizontal input won't affect wallJump")] [SerializeField] bool autoHorizontal = false;
    [Tooltip("This gives us X amount of time after leaving wall to wall jump")] [SerializeField] float wallJumpTime = .2f;
    [SerializeField] float wallJumpDur = .4f;
    [SerializeField] Vector2 wallJumpPower = new Vector2(8f, 16f);
    
    /* PRIVATE VARS */

    private bool isFacingRight = true;
    private bool wallSliding = false;
    private float horizontal;
    private Rigidbody2D rb;
    private int jumpsLeft;

    // wall jumping vars
    private bool isWallJumping = false;
    private float wallJumpDir;
    private float wallJumpCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = amountOfJumps;
    }

    private void Update()
    {
        MoveLogic();

        WallSlide();
        WallJump();

        if(!isWallJumping || !autoHorizontal)
            Flip();
    }

    private void MoveLogic()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * rb.gravityScale);
            jumpsLeft--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * rb.gravityScale * secondaryJumpModifiers);
            jumpsLeft--;
        }
        if (IsGrounded())
        {
            jumpsLeft = amountOfJumps;
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping || !autoHorizontal)
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayers);
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1f;
            transform.localScale = tempScale;
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, .2f, wallLayers);
    }
    private void WallSlide()
    {
        if(IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            wallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue)); 
        }
        else
        {
            wallSliding = false;
        }
    }

    private void WallJump()
    {
        if (wallSliding)
        {
            isWallJumping = false;
            wallJumpDir = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpCounter > 0)
        {
            isWallJumping = true;
            if(autoHorizontal)
                rb.velocity = new Vector2(wallJumpDir * wallJumpPower.x, wallJumpPower.y);
            else
                rb.velocity = new Vector2(rb.velocity.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if(transform.localScale.x != wallJumpDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 tempScale = transform.localScale;
                tempScale.x *= -1f;
                transform.localScale = tempScale;
            }
            Invoke(nameof(StopWallJump), wallJumpDur);
        }
    }
    private void StopWallJump()
    {
        isWallJumping = false;
    }
}
