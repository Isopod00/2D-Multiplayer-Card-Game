using System.Collections.Generic;
using UnityEngine;

// This class defines the database of cards in the game
public class CardDataBase : MonoBehaviour
{
    // Create a new list of Card objects
    private static List<Card> cardList = new List<Card>();

    void Awake()
    {
        // List all minions cards in the game along with their attributes
        cardList.Add(new MinionCard(0, "Anthony", 1, "None", 10, 1));
        cardList.Add(new MinionCard(1, "Evan", 1, "None", 20, 2));
        cardList.Add(new MinionCard(2, "Daniel", 1, "None", 30, 3));
        cardList.Add(new MinionCard(3, "Timmy", 2, "None", 40, 4));
        cardList.Add(new MinionCard(4, "Spencer", 2, "None", 50, 5));
        cardList.Add(new MinionCard(5, "Aaron", 3, "None", 60, 6));

        // List all building cards in the game along with their attributes
        cardList.Add(new BuildingCard(6, "Hut", 1, "None", 60, 2));
        cardList.Add(new BuildingCard(7, "Temple", 2, "None", 70, 3));
        cardList.Add(new BuildingCard(8, "Cathedral", 3, "None", 80, 5));
        cardList.Add(new BuildingCard(9, "Collesseum", 5, "None", 90, 7));
    }

    // Public Getter Method for returning the database
    public static List<Card> getCardDatabase()
    {
        return cardList;
    }
}