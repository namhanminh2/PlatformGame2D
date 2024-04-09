using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rhino : Enemy
{
    [Header("Rhino specific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
                     private float shockTimeCounter;


    protected override void Start()
    {
        base.Start();
        invincible = true;
    }


    void Update()
    {
        CollisionCheck();

        AnimatorController();
        if (!playerDetection)
        {
            WalkAround();
            return;
        }

        if (playerDetection.collider.GetComponent<Player>() != null && playerDetection)
            aggresive = true;

        if (!aggresive)
        {
            WalkAround();
        }
        else
        {
            if (!groundDetected && !wallDetected)
            {
                aggresive = false;
                Flip();
            }

            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);



            if (wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }

            if (shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                aggresive = false;
            }
            shockTimeCounter -= Time.deltaTime;
        }

        
    }

    private void AnimatorController()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

}
