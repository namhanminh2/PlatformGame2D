using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Danger
{

    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDirection = -1;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatToIgnore;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    protected bool wallDetected;
    protected bool groundDetected;
    protected RaycastHit2D playerDetection;
    protected Transform player;

    [HideInInspector] public bool invincible;

    [Header("FX")]
    [SerializeField] private GameObject deathFx;

    [Header("Move info")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime = 2;
                     protected float idleTimeCounter;

    protected bool canMove = true;
    protected bool aggresive;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("FindPlayer", 0, .5f);
        FindPlayer();

        if (groundCheck == null)
            groundCheck = transform;
        if (wallCheck == null)
            wallCheck = transform;
    }

    public virtual void Damage()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("gotHit");
        }
    }

    private void FindPlayer()
    {
        if (player != null)
            return;

        if (PlayerManager.instance.currentPlayer != null)
            player = PlayerManager.instance.currentPlayer.transform;
    }

    public void DestroyMe()
    {
        if (deathFx != null)
        {
            GameObject newDeathFx = Instantiate(deathFx, transform.position, transform.rotation);
            Destroy(newDeathFx, .3f);
        }

        if (GetComponent<Enemy_DropController>() != null)
            GetComponent<Enemy_DropController>().DropFruits();
        else
            Debug.LogWarning("You don't have !Enemy_DropController! on the enemy!");

        Destroy(gameObject);
    }

    protected virtual void WalkAround()
    {
        if (idleTimeCounter <= 0 && canMove)
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, 0);

        idleTimeCounter -= Time.deltaTime;

        if (!groundDetected || wallDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionCheck()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 100, ~whatToIgnore);
    }

    protected virtual void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        
        if (wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));

        }

    }
}
