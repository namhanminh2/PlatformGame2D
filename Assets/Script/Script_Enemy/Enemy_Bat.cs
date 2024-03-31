using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat Specific")]
    [SerializeField] private Transform[] idlePoint;


    private Vector2 destination;
    private bool canBeAgressive = true;
    private bool playerDetected;
    private Transform player;
    float defaultSpeed;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
    }


    // Update is called once per frame
    void Update()
    {
        anim.SetBool("canBeAgressive", canBeAgressive);
        anim.SetFloat("speed", speed);

        idleTimeCounter -= Time.deltaTime;
        if (idleTimeCounter > 0)
            return;

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

        if (playerDetected && !aggresive && canBeAgressive)
        {
            aggresive = true;
            canBeAgressive = false;
            destination = player.transform.position;
        }


        if (aggresive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                aggresive = false;

                int i = Random.Range(0, idlePoint.Length);

                destination = idlePoint[i].position;
                speed = speed * .5f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                if (!canBeAgressive)
                    idleTimeCounter = idleTime;

                canBeAgressive = true;
                speed = defaultSpeed;

            }

        }

        FlipController();

    }

    public override void Damage()
    {
        base.Damage();

        idleTimeCounter = 5;
    }

    private void FlipController()
    {
        if (facingDirection == -1 && transform.position.x < destination.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > destination.x)
            Flip();
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
