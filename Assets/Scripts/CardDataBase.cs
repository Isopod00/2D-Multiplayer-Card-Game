using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines the database of cards in the game
public class CardDataBase : MonoBehaviour
{

    // Create a new list of Card objects
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        // List all cards in the game along with their attributes
        cardList.Add(new Card(0, "Anthony", 1, 10, "None"));
        cardList.Add(new Card(1, "Evan", 1, 20, "None"));
        cardList.Add(new Card(2, "Daniel", 2, 30, "None"));
        cardList.Add(new Card(3, "Timmy", 2, 40, "None"));
        cardList.Add(new Card(4, "Spencer", 3, 50, "None"));
        cardList.Add(new Card(5, "Aaron", 3, 60, "None"));
        cardList.Add(new Card(6, "Josh", 4, 80, "None"));
        cardList.Add(new Card(7, "Peter", 4, 90, "None"));
        cardList.Add(new Card(8, "Luke", 5, 115, "None"));
    }
}