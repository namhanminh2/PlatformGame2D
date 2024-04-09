using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant specific")]

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool facingRight;

    protected override void Start()
    {
        base.Start();
        if (facingRight)
            Flip();
    }


    void Update()
    {

        CollisionCheck();
        idleTimeCounter -= Time.deltaTime;
        bool playerDectected = playerDetection.collider.GetComponent<Player>() != null;

        if (idleTimeCounter < 0 && playerDectected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
        }

    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, 3f);
    }
}
