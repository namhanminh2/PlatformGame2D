using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBird : Enemy
{
    private RaycastHit2D ceillingDetected;

    [Header("Blue Bird specific")]
    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;

    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    private float flyForce;
    private bool canFly = true;


    public override void Damage()
    {
        canFly = false;
        rb.velocity = new Vector2 (0, 0);
        rb.gravityScale = 0;
        base.Damage();
    }


    protected override void Start()
    {
        base.Start();
        flyForce = flyUpForce;
    }

    [SerializeField] private Transform movepoint;
    [SerializeField] private float xMultiplier;
    [SerializeField] private float yMultiplier;


    public void FlyUpEvent()
    {
        if (canFly)
            rb.velocity = new Vector2(speed * facingDirection, flyForce);

        //if (canFly)
        //{
        //    Vector2 direction = transform.position - movepoint.position;
        //    rb.velocity = new Vector2(-direction.x * xMultiplier, -direction.y * yMultiplier);
        //}

    }

    // Update is called once per frame
    void Update()
    {
        CollisionCheck();

        if (ceillingDetected)
            flyForce = flyDownForce;
        else if (groundDetected)
            flyForce = flyUpForce;

        if (wallDetected)
            Flip();
    }
    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));

    }
}
