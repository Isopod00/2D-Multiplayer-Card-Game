using UnityEngine;
using Mirror;

public class DrawCards : NetworkBehaviour
{
    private PlayerManager playerManager;

    private GameObject playerArea;
    private int maxHandSize = 10; // Specify the maximum hand size

    public void OnClick()
    {
        playerArea = GameObject.Find("PlayerArea");

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        if (playerArea.transform.childCount < maxHandSize && playerManager.myTurn())
        {
            playerManager.CmdDrawCards(1); // Ask the server to carry out this command
        } 
    }
}