using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_DroppedByEnemy : Fruit_DropedByPlayer
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2[] dropDirection;
    [SerializeField] private float force;

    protected override void Start()
    {

        rb = GetComponentInParent<Rigidbody2D>();
        base.Start();

        int random = Random.Range(0, dropDirection.Length);
        rb.velocity = dropDirection[random] * force;

    }

    protected override IEnumerator BlinkImage()
    {
        anim.speed = 0;
        sr.color = transperentColor;


        yield return new WaitForSeconds(.1f);
        sr.color = Color.white;


        yield return new WaitForSeconds(.1f);
        sr.color = transperentColor;




        yield return new WaitForSeconds(.1f);
        sr.color = Color.white;


        yield return new WaitForSeconds(.1f);
        sr.color = transperentColor;




        yield return new WaitForSeconds(.2f);
        sr.color = Color.white;


        yield return new WaitForSeconds(.2f);
        sr.color = transperentColor;

        yield return new WaitForSeconds(.1f);
        sr.color = Color.white;

        anim.speed = 1;
        canPickUp = true;
    }
}
