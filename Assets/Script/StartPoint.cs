using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class StartPoint : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.transform.position.x > transform.position.x)
                GetComponent<Animator>().SetTrigger("Start1");
        }
    }
}
