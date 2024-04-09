using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [HideInInspector] public int fruits;
    [HideInInspector] public Transform respawnPoint;
    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public int choosenSkinId;

    /*public InGame_UI inGameUI;*/
    [Header("Player info")]

    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject deathFx;

    [Header("Camera Shake FX")]
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private Vector3 shakeDirection;
    [SerializeField] private float forceMultiplier;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            PlayerRespawn();
    }

    public void OnFalling()
    {
        KillPlayer();

        int difficulty = GameManager.instance.difficulty;

        if (difficulty < 3)
        {
            Invoke("RespawnPlayer", 1);

            if (difficulty > 1)
                HaveEnoughFruits();
        }
        /*else
            inGameUI.OnDeath();*/

    }

    private void DropFruit()
    {
        int fruitIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(FruitType)).Length);

        GameObject newFruit = Instantiate(fruitPrefab, currentPlayer.transform.position, transform.rotation);
        newFruit.GetComponent<Fruit_DropedByPlayer>().FruitSteup(fruitIndex);
        Destroy(newFruit, 20);
    }

    private bool HaveEnoughFruits()
    {
        if (fruits > 0)
        {
            fruits--;

            if (fruits < 0)
                fruits = 0;

            DropFruit();
            return true;
        }

        return false;
    }

    public void PlayerRespawn()
    {
        if ( currentPlayer == null)
        {
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
        }
    }


    public void KillPlayer()
    {
        /*AudioManager.instance.PlaySFX(0);*/

        GameObject newDeathFx = Instantiate(deathFx, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(newDeathFx, .4f);
        Destroy(currentPlayer);
    }











}
