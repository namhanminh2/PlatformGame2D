using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private InGame inGame;
    private void Start()
    {
        inGame = GameObject.Find("Canvas").GetComponent<InGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GetComponent<Animator>().SetTrigger("activated");

            AudioManager.instance.PlaySFX(2);
            PlayerManager.instance.KillPlayer();

            inGame.OnLevelFinished();

            GameManager.instance.SaveBestTime();
            GameManager.instance.SaveCollectedFruits();
            GameManager.instance.SaveLevelInfo();
        }
    }
}
