using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    private void Update()
    {
        WalkAround();

        CollisionCheck();
        
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
