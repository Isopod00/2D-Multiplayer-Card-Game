using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

// This class defines the basic template for a Card object
public class Card {
    
    private int cardID;
    private string cardName;
    private int cardCost;
    private int cardHealth;
    private string cardDescription;

    // Class Constructor
    public Card(int ID, string Name, int Cost, int Health, string Description) {
        cardID = ID;
        cardName = Name;
        cardCost = Cost;
        cardHealth = Health;
        cardDescription = Description;
    }

    // Getter Methods
    public int getID() {
        return cardID;
    }
    public string getName() {
        return cardName;
    }
    public int getCost() {
        return cardCost;
    }
    public int getHealth() {
        return cardHealth;
    }
    public string getDescription() {
        return cardDescription;
    }
}