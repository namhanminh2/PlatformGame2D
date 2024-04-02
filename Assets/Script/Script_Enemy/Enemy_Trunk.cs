using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{

    [Header("Trunk specific")]
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform groundBehindCheck;
    private bool wallBehind;
    private bool groundBehind;



    private bool playerDetected;

    [Header("Bullet specific")]
    [SerializeField] private float attackCooldown;
    private float attackCooldownCounter;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;


    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CollisionCheck();

        if (!canMove)
            rb.velocity = new Vector2(0, 0);

        attackCooldownCounter -= Time.deltaTime;
        if (playerDetection.collider.GetComponent<Player>() != null)
        {
            if (attackCooldownCounter < 0)
            {
                attackCooldownCounter = attackCooldown;
                anim.SetTrigger("attack");
                canMove = false;
            }
            else if (playerDetection.distance < 3)
                MoveBackwards(1.5f);
        }
        else
        {
            if (playerDetected)
                MoveBackwards(4);
            else
                WalkAround();

        }

        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void MoveBackwards(float multiplier)
    {
        if (wallBehind)
            return;
        if (!groundBehind)
            return;

        rb.velocity = new Vector2(speed * multiplier * -facingDirection, rb.velocity.y);
    }



    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
    }

    private void ReturnMovement()
    {
        canMove = true;
    }


    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        groundBehind = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallBehind = Physics2D.Raycast(wallCheck.position, Vector2.right * (-facingDirection + 1), wallCheckDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);

        Gizmos.DrawLine(groundBehindCheck.position, new Vector2(groundBehindCheck.position.x, groundBehindCheck.position.y - groundCheckDistance));

    }

}
