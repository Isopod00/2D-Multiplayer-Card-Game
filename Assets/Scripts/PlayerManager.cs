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
    private Text playerGold; // For displaying how much gold the player has
    private Text turnText;

    private int TurnsPlayed = 0; // Initialize the number of turns played
    private int gold = 2; // Define the starting gold for each player

    private bool isMyTurn; // Keep track of who's turn it currently is

    public override void OnStartClient()
    {
        base.OnStartClient();

        // If player 1
        if (Mirror.NetworkServer.connections.ContainsKey(0))
        {
            isMyTurn = true;
        } 
        // If player 2
        else
        {
            isMyTurn = false;
        }

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
        playerGold = GameObject.Find("Gold Text").GetComponent<Text>();
        playerGold.text = "Gold: " + gold; // Display the initial amount of gold
        turnText = GameObject.Find("Turn Text").GetComponent<Text>();

        if(isMyTurn)
        {
            turnText.text = "Your Turn!";
            turnText.color = new Color(0, 255, 0);
        } else
        {
            turnText.text = "Opponent's Turn";
            turnText.color = new Color(255, 0, 0);
        }
    }

    // Public Getter Method for the current client's gold
    public int getGold()
    {
        return gold;
    }
    // Public Getter Method for if its the current player's turn
    public bool myTurn()
    {
        return isMyTurn;
    }

    public void toggleMyTurn()
    {
        if (isMyTurn)
        {
            // Find all cards currently played by the client
            foreach (Transform transform in playerDropZone.transform)
            {
                // Gain gold for your played buildings!
                GameObject card = transform.gameObject;
                ThisCard script = card.GetComponent<ThisCard>();
                Card thisCard = script.getThis();
                if (thisCard is BuildingCard)
                {
                    BuildingCard building = (BuildingCard)thisCard;
                    gold += building.getIncome();
                }
            }
            isMyTurn = false;
            turnText.text = "Opponent's Turn";
            turnText.color = new Color(255, 0, 0);
        } else
        {
            isMyTurn = true;
            turnText.text = "Your Turn!";
            turnText.color = new Color(0, 255, 0);
        }
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
    private void RpcDecrementDeck()
    {
        deckSize--; // Decrease deck size by 1
    }

    // This RPC (Remote Procedure Call) makes sure that cards are properly displayed for each client
    [ClientRpc]
    private void RpcShowCard(GameObject card, string type)
    {
        if (type == "dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(playerArea.transform, false);

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
                GameManager.changeTurns(); // End your turn after playing a card

                ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
                gold -= script.getThis().getCost();
                playerGold.text = "Gold: " + gold; // Display the current amount of gold
            } else
            {
                card.transform.SetParent(enemyDropZone.transform, false);
                card.GetComponent<CardFlipper>().Flip();
                GameManager.changeTurns(); // End your turn after playing a card
            }
        }
    }
}