using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] public bool testingOnPc;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dustFx;
    private float dustFxTimer;

    private bool facingRight=true;
    private int facingDirection = 1;
    

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;
    public Vector2 wallJumpDirection;


    private float defaultJumpForce;

    private bool readyToLand;

    private bool canDoubleJump;
    private bool canMove = true;

    private bool canBeControlled;
    private float defaultGravityScale;


    [SerializeField] private float bufferJumpTime;
    private float bufferJumpCounter;
    [SerializeField] private float coyoteJumpTime;
    private float coyoteJumpCounter;
    private bool canHavaCoyoteJump;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockBackDirection;
    [SerializeField] private float knockBackTime;
    [SerializeField] private float knockBackProtectionTime;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    private bool isGrounded;
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;

    [Header("Controlls info")]
    public VariableJoystick joystick;
    private float movingInput;
    private float vInput;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SetAnimationLayer();

        defaultJumpForce = jumpForce;
        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorController();

        if (isKnocked)
            return;

        CollisionCheck();
        FlipController();
        CheckInput();
        CheckForEnemy();

        bufferJumpCounter -= Time.deltaTime;
        coyoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            canMove = true;
            canDoubleJump = true;
            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }
            canHavaCoyoteJump = true;

            if (readyToLand)
            {
                dustFx.Play();
                readyToLand = false;
            }
        }
        else
        {
            if (!readyToLand)
                readyToLand = true;

            if (canHavaCoyoteJump)
            {
                canHavaCoyoteJump = false;
                coyoteJumpCounter = coyoteJumpTime;
            }
        }


        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }

        Move();
    }

    private void SetAnimationLayer()
    {
        int skinIndex = PlayerManager.instance.choosenSkinId;

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(skinIndex, 1);
    }

    private void CheckForEnemy()
    {
        Collider2D[] hitedColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitedColliders)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {

                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if (newEnemy.invincible)
                    return;

                if (rb.velocity.y < 0)
                {
                    AudioManager.instance.PlaySFX(1);
                    newEnemy.Damage();
                    anim.SetBool("flipping", true);
                    Jump();
                }
            }
        }
    }
    private void StopFlippingAnimation()
    {
        anim.SetBool("flipping", false);
    }

    private void WallJump()
    {
        AudioManager.instance.PlaySFX(3);
        canMove = false;
        canDoubleJump = true;

        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
        dustFx.Play();
    }

    private void CheckInput()
    {
        if (!canBeControlled)
            return;

        if (testingOnPc)
        {
            movingInput = Input.GetAxisRaw("Horizontal");
            vInput = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movingInput = joystick.Horizontal;
            vInput = joystick.Vertical;
        }

        if (vInput < 0)
            canWallSlide = false;

        if (Input.GetKeyDown(KeyCode.Space))
            JumpButton();
    }

    public void ReturnControll()
    {
        rb.gravityScale = defaultGravityScale;
        canBeControlled = true;
    }

    public void KnockBack(Transform damageTransform)
    {
        AudioManager.instance.PlaySFX(9);
        if (!canBeKnocked)
            return;

        if (GameManager.instance.difficulty > 1)
            PlayerManager.instance.OnTakingDamage();

        PlayerManager.instance.ScreenShake(-facingDirection);

        isKnocked = true;
        canBeKnocked = false;

        #region Define horizontal direction for knockback
        int hDirection = 0;
        if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        
        else if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        
        #endregion

        rb.velocity = new Vector2(knockBackDirection.x * hDirection, knockBackDirection.y);

        Invoke("CancelKnockBack", knockBackTime);
        Invoke("AllowKnockBack", knockBackProtectionTime);
    }

    private void CancelKnockBack()
    {
        isKnocked = false;
    }

    private void AllowKnockBack()
    {
        canBeKnocked = true;
    }

    private void Move()
    {
        if ( canMove)
            rb.velocity = new Vector2(movingInput * moveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if (dustFxTimer < 0)
        {
            dustFx.Play();
            dustFxTimer = .7f;
        }

        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FlipController()
    {
        dustFxTimer -= Time.deltaTime;

        if (isGrounded && isWallDetected)
        {
            if (facingRight && movingInput < 0)
                Flip();
            else if (!facingRight && movingInput > 0)
                Flip();
        }

        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if(rb.velocity.x < 0 && facingRight)
            Flip();
    }

    public void JumpButton()
    {
        if(!isGrounded)
        {
            bufferJumpCounter = bufferJumpTime;
        }

        if(isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }
        else if (isGrounded || coyoteJumpCounter > 0)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }
        canWallSlide = false;
    }
    private void Jump()
    {
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (isGrounded)
            dustFx.Play();
    }

    public void Push(float pushForce)
    {

        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetBool("isKnocked", isKnocked);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("canBeControlled", canBeControlled);
        anim.SetBool("isWallDetected", isWallDetected);
    }
    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsWall);

        if(!isGrounded && rb.velocity.y <0)
            canWallSlide = true;

        if (!isWallDetected)
        {
            canWallSlide = false;
            isWallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);

    }
}
