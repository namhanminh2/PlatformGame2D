using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject myCamera;
    [SerializeField] private PolygonCollider2D cd;
    [SerializeField] private Color gizmosColor;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myCamera.GetComponent<CinemachineVirtualCamera>().Follow = PlayerManager.instance.currentPlayer.transform;

            myCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            myCamera.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(cd.bounds.center, cd.bounds.size);
    }
}
