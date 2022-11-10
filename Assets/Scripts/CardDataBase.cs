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
        cardList.Add(new MinionCard(2, "Daniel", 2, "None", 30, 3));
        cardList.Add(new MinionCard(3, "Timmy", 2, "None", 40, 4));
       /* cardList.Add(new MinionCard(4, "Spencer", 3, "None", 50, 5));
        cardList.Add(new MinionCard(5, "Aaron", 3, "None", 60, 6));
        cardList.Add(new MinionCard(6, "Josh", 4, "None", 70, 7));
        cardList.Add(new MinionCard(7, "Peter", 4, "None", 80, 8));
        cardList.Add(new MinionCard(8, "Luke", 5, "None", 90, 9));*/

        // List all building cards in the game along with their attributes
        cardList.Add(new BuildingCard(4, "Hut", 1, "None", 60, 3));
        /*cardList.Add(new BuildingCard(10, "Temple", 3, "None", 70, 4));
        cardList.Add(new BuildingCard(11, "Cathedral", 4, "None", 80, 5));
        cardList.Add(new BuildingCard(12, "Collesseum", 5, "None", 90, 6));*/
    }

    // Public Getter Method for returning the database
    public static List<Card> getCardDatabase()
    {
        return cardList;
    }
}