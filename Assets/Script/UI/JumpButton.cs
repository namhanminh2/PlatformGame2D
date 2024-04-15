using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class JumpButton : MonoBehaviour, IPointerDownHandler
{
    private Player player;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerManager.instance.currentPlayer != null)
        {
            player = PlayerManager.instance.currentPlayer.GetComponent<Player>();
            player.JumpButton();
        }
    }
}
