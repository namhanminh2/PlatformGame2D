using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class FruitManager : MonoBehaviour
{
    [SerializeField] private Transform[] fruitPosition;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private bool randomFruits;

    private int fruitIndex;
    void Start()
    {
        fruitPosition = GetComponentsInChildren<Transform>();

        for (int i = 1; i < fruitPosition.Length; i++)
        {
            GameObject newFruit = Instantiate(fruitPrefab, fruitPosition[i]);

            if (randomFruits)
            {
                fruitIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(FruitType)).Length);
                newFruit.GetComponent<Fruit_Items>().FruitSteup(fruitIndex);
            }
            else
            {
                newFruit.GetComponent<Fruit_Items>().FruitSteup(fruitIndex);
                fruitIndex++;

                if (fruitIndex > Enum.GetNames(typeof(FruitType)).Length)
                    fruitIndex = 0;
            }

            fruitPosition[i].GetComponent<SpriteRenderer>().sprite = null;

            /*int levelNumber = GameManager.instance.levelNumber;
            int totalAmountOfFruits = PlayerPrefs.GetInt("Level" + levelNumber + "TotalFruits");

            if (totalAmountOfFruits != fruitPosition.Length - 1)
                PlayerPrefs.SetInt("Level" + levelNumber + "TotalFruits", fruitPosition.Length - 1);*/
        }
    }
}
