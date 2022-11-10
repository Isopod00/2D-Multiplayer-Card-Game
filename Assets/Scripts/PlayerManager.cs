using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject minionCardObject;
    public GameObject buildingCardObject;

    private GameObject playerArea;
    private GameObject enemyArea;
    private GameObject playerDropZone;
    private GameObject enemyDropZone;

    private List<Card> deck = new List<Card>(); // Create a new list of Card objects
    private int deckSize = 20; // Default deck size is defined here

    private Text cardsLeft; // For displaying how many cards are in the player's deck
    private int TurnsPlayed = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Fill the deck with random cards from the database
        for (int i = 0; i < deckSize; i++)
        {
            deck.Add(CardDataBase.getCardDatabase()[Random.Range(0, CardDataBase.getCardDatabase().Count)]);
        }

        // Find our GameObjects within the scene
        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        playerDropZone = GameObject.Find("PlayerDropZone");
        enemyDropZone = GameObject.Find("EnemyDropZone");

        cardsLeft = GameObject.Find("CardsLeft").GetComponent<Text>();
        cardsLeft.text = deckSize.ToString(); // Display the initial number of cards in the deck
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Command]
    // Network Command for shuffling the deck
    public void CmdShuffle()
    {
        Card temp;

        for (int i = 0; i < deckSize; i++)
        {
            int swapWith = Random.Range(0, deckSize);

            // Swap the cards to shuffle
            temp = deck[swapWith];
            deck[swapWith] = deck[i];
            deck[i] = temp;
        }
    }

    [Command]
    // Network Command for drawing cards
    public void CmdDrawCards(int amountToDraw)
    {
        // Make sure we don't draw more than what is remaining in the deck
        if (amountToDraw > deckSize)
        {
            amountToDraw = deckSize;
        }

        for (int i = 0; i < amountToDraw; i++)
        {
            if (deck[0] is MinionCard)
            {
                GameObject card = Instantiate(minionCardObject, new Vector2(0, 0), Quaternion.identity);
                ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
                script.cardID = deck[0].getID(); // Set the proper card ID for the new card object

                NetworkServer.Spawn(card, connectionToClient); // Spawn the card for all clients on the network
                RpcDecrementDeck(); // Decrement the deck using an Rpc so the variable is updated for each client seperately
                deck.RemoveAt(0); // Remove the drawn card from our deck
                RpcShowCard(card, "dealt"); // Display the card properly for each client
            } else if (deck[0] is BuildingCard)
            {
                GameObject card = Instantiate(buildingCardObject, new Vector2(0, 0), Quaternion.identity);
                ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
                script.cardID = deck[0].getID(); // Set the proper card ID for the new card object

                NetworkServer.Spawn(card, connectionToClient); // Spawn the card for all clients on the network
                RpcDecrementDeck(); // Decrement the deck using an Rpc so the variable is updated for each client seperately
                deck.RemoveAt(0); // Remove the drawn card from our deck
                RpcShowCard(card, "dealt"); // Display the card properly for each client
            }
        }
    }

    [Command]
    public void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "played");
    }

    // The reason for making this trivial task its own Rpc is so the variables are updated for each client
    [ClientRpc]
    void RpcDecrementDeck()
    {
        deckSize--; // Decrease deck size by 1
    }

    // This RPC (Remote Procedure Call) makes sure that cards are properly displayed for each client
    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerArea.transform, false);

                cardsLeft = GameObject.Find("CardsLeft").GetComponent<Text>();
                cardsLeft.text = deckSize.ToString(); // Display the current number of cards left
            } else
            {
                card.transform.SetParent(enemyArea.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        } else if (type == "played")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerDropZone.transform, false);
                TurnsPlayed++;
                Debug.Log("Turns Played: " + TurnsPlayed);
            } else
            {
                card.transform.SetParent(enemyDropZone.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
    }
}