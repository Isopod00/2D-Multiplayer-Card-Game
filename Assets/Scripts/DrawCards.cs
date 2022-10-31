using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager playerManager;
    public GameObject playerArea;

    private int maxHandSize = 10; // Specify the maximum hand size

    public void OnClick()
    {
        playerArea = GameObject.Find("PlayerArea");

        if (playerArea.transform.childCount < maxHandSize)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.CmdDrawCards(1); // Ask the server to carry out this command
        } 
    }
}