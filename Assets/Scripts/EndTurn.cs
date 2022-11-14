using Mirror;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    private PlayerManager playerManager;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        if(playerManager.myTurn())
        {
            playerManager.CmdEndTurn();
        }
    }
}