using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

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

    private int maxHandSize = 10;
    private int currentHandSize = 0;

    private Text cardsLeft; // For displaying how many cards are in the player's deck
    private Text playerGold; // For displaying how much gold the player has
    private Text turnText;

    private int TurnsPlayed = 0; // Initialize the number of turns played
    private int gold = 0; // Define the starting gold for each player

    private bool isMyTurn; // Keep track of who's turn it currently is

    public override void OnStartClient()
    {
        base.OnStartClient();

        // If player 1
        if (Mirror.NetworkServer.connections.ContainsKey(0))
        {
            isMyTurn = false;
        } 
        // If player 2
        else
        {
            isMyTurn = true;
        }

        // Fill the deck with random cards from the database
        for (int i = 0; i < deckSize; i++)
        {
            deck.Add(CardDataBase.getCardDatabase()[UnityEngine.Random.Range(0, CardDataBase.getCardDatabase().Count)]);
        }

        gold = 2;

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

        // Wait until both players conect to draw the initial cards
        if(NetworkServer.connections.Count == 2) 
        {
            GameManager.drawInitialCards();
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
            TurnsPlayed++;
            Debug.Log("Turns Played: " + TurnsPlayed);

            isMyTurn = false;
            turnText.text = "Opponent's Turn";
            turnText.color = new Color(255, 0, 0);
        } else
        {
            isMyTurn = true;
            turnText.text = "Your Turn!";
            turnText.color = new Color(0, 255, 0);

            if(hasAuthority)
            {
                CmdCollectGold(); // Tally up the gold from your current buildings
                CmdDrawCards(2); // Draw two cards
            }
        }
    }

    [Command]
    public void CmdCollectGold()
    {
        RpcCollectGold();
    }
    [ClientRpc]
    private void RpcCollectGold()
    {
        if(hasAuthority)
        {
            // Find all cards currently played by the client
            foreach (Transform transform in playerDropZone.transform)
            {
                // Gain gold for your played buildings!
                GameObject cardObject = transform.gameObject;
                ThisCard script = cardObject.GetComponent<ThisCard>();
                Card thisCard = script.getThis();
                if (thisCard is BuildingCard)
                {
                    BuildingCard building = (BuildingCard)thisCard;
                    gold += building.getIncome();
                }
            }
            playerGold.text = "Gold: " + gold; // Display the current amount of gold
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
            int swapWith = UnityEngine.Random.Range(0, deckSize);

            // Swap the cards to shuffle
            temp = deck[swapWith];
            deck[swapWith] = deck[i];
            deck[i] = temp;
        }
    }

    [Command(requiresAuthority = false)]
    // Network Command for drawing cards
    public void CmdDrawCards(int amountToDraw)
    {
        // Make sure we don't draw more than what is remaining in the deck
        if (amountToDraw > deckSize)
        {
            amountToDraw = deckSize;
        }

        if(currentHandSize + amountToDraw > maxHandSize)
        {
            amountToDraw = Math.Max(0, maxHandSize - currentHandSize);
        }

        for (int i = 0; i < amountToDraw; i++)
        {
            if (deck[0] is MinionCard)
            {
                currentHandSize++;
                GameObject card = Instantiate(minionCardObject, new Vector2(0, 0), Quaternion.identity);
                ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
                script.cardID = deck[0].getID(); // Set the proper card ID for the new card object

                NetworkServer.Spawn(card, connectionToClient); // Spawn the card for all clients on the network
                RpcDecrementDeck(); // Decrement the deck using an Rpc so the variable is updated for each client seperately
                deck.RemoveAt(0); // Remove the drawn card from our deck
                RpcShowCard(card, "dealt"); // Display the card properly for each client
            } else if (deck[0] is BuildingCard)
            {
                currentHandSize++;
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
        currentHandSize--;
        RpcShowCard(card, "played");
    }

    // The reason for making this trivial task its own Rpc is so the variables are updated for each client
    [ClientRpc]
    private void RpcDecrementDeck()
    {
        deckSize--; // Decrease deck size by 1
    }

    [Command]
    public void CmdEndTurn()
    {
        RpcEndTurn();
    }
    [ClientRpc]
    private void RpcEndTurn()
    {
        GameManager.changeTurns(); // End your turn
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

                ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
                gold -= script.getThis().getCost();
                playerGold.text = "Gold: " + gold; // Display the current amount of gold
            } else
            {
                card.transform.SetParent(enemyDropZone.transform, false);
                card.GetComponent<CardFlipper>().Flip();
            }
        }
    }
}