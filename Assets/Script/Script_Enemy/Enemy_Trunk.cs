using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{

    [Header("Trunk specific")]
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;



    private bool playerDetected;

    [Header("Bullet specific")]
    [SerializeField] private float attackCooldown;
    private float attackCooldownCounter;


    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CollisionCheck();
        

        attackCooldownCounter -= Time.deltaTime;
        if(playerDetection.collider.GetComponent<Player>() != null)
        {
            if(attackCooldownCounter < 0 )
            {
                attackCooldownCounter = attackCooldown;
                anim.SetTrigger("attack");
                canMove = false;
            }
        }
        else
        {
        WalkAround();

        }
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

    }



}
