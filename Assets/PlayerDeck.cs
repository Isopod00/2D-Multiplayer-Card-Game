using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class creates the basic structure of a deck
public class PlayerDeck : MonoBehaviour {

    public List<Card> deck = new List<Card>();
    public int deckSize = 40; // Default deck size is defined here

    public GameObject PlayerArea;
    public GameObject cardObject;

    // Start is called before the first frame update
    void Start() {
        // Fill the deck with random cards from the database
        for(int i = 0; i < deckSize; i++) {
            deck.Add(CardDataBase.cardList[Random.Range(0, CardDataBase.cardList.Count)]);
        }
    }

    // Method for shuffling the deck
    public void shuffle() {
        Card temp;

        for(int i = 0; i < deckSize; i++) {
            int swapWith = Random.Range(0, deckSize);

            // Swap the cards to shuffle
            temp = deck[swapWith];
            deck[swapWith] = deck[i];
            deck[i] = temp;
        }
    }

    // Method for drawing cards
    public void drawCards(int amountToDraw) {
        // Make sure we don't draw more than what is remaining in the deck
        if(amountToDraw > deckSize) {
            amountToDraw = deckSize;
        }

        for(int i = 0; i < amountToDraw; i++) {
            // Spawn a new card object
            GameObject card = Instantiate(cardObject, new Vector2(0, 0), Quaternion.identity);
            card.transform.SetParent(PlayerArea.transform, false);

            ThisCard script = card.GetComponent<ThisCard>(); // Access this script from the new card object
            script.cardID = deck[i].getID(); // Set the proper card ID for the new card object

            deck.RemoveAt(i); // Remove the drawn card from our deck
            deckSize--; // Decrease deck size by 1
        }
    }
}