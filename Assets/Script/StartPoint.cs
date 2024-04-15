using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Transform respPoint;

    private void Awake()
    {

    }
    private void Start()
    {
        PlayerManager.instance.respawnPoint = respPoint;
        PlayerManager.instance.RespawnPlayer();

        PlayerManager.instance.fruits = 0;
        GameManager.instance.timer = 0;

        AudioManager.instance.PlayRandomBGM();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (!GameManager.instance.startTime)
                GameManager.instance.startTime = true;

            if (collision.transform.position.x > transform.position.x)
                GetComponent<Animator>().SetTrigger("touch");
        }
    }


}
