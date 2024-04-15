using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DropController : MonoBehaviour
{
    [SerializeField] private GameObject fruit;

    [Range(1, 10)]
    [SerializeField] private int dropAmount;

    public void DropFruits()
    {
        for (int i = 0; i < dropAmount; i++)
        {
            GameObject newFruit = Instantiate(fruit, transform.position, transform.rotation);
            Destroy(newFruit, 10);
        }
    }
}
