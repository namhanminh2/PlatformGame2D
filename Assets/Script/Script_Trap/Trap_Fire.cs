using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : Danger
{
    
    public bool isWorking;
    private Animator anim;
    public float repeatRate;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if(transform.parent == null)
        {
            InvokeRepeating("FireSwitch", 0, repeatRate);
        }
    }

    private void Update()
    {
        anim.SetBool("isWorking", isWorking);
    }

    public void FireSwitch()
    {
        isWorking = !isWorking;
    }

    public void FireSwitchAfter(float seconds)
    {
        CancelInvoke();
        isWorking = false;
        Invoke("FireSwitch", seconds);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWorking)
            base.OnTriggerEnter2D(collision);
    }
}
