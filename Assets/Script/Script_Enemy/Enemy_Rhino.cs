using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rhino : Enemy
{
    [Header("Rhino spesific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
                     private float shockTimeCounter;


    protected override void Start()
    {
        base.Start();
        invincible = true;
    }


    // Update is called once per frame
    void Update()
    {
        CollisionCheck();
        
        if (playerDetection.collider.GetComponent<Player>() != null)
            aggresive = true;

        if (!aggresive)
        {
            WalkAround();
        }
        else
        {
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


        AnimatorController();
    }

    private void AnimatorController()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
