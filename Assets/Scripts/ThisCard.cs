using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// This class is meant to represent a single card
public class ThisCard : MonoBehaviour
{

    public int cardID;

    private Card thisCard;
    private int thisID;

    // These are the Text fields for card GameObjects
    public Text nameText;
    public Text costText;
    public Text healthText;
    public Text descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        thisCard = CardDataBase.cardList[cardID];
        thisID = cardID;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the card ID has been changed, and update the card if it has
        if (thisID != cardID)
        {
            thisID = cardID;
            thisCard = CardDataBase.cardList[cardID];
        }

        // Update the card Text fields
        nameText.text = thisCard.getName();
        costText.text = thisCard.getCost().ToString();
        healthText.text = thisCard.getHealth().ToString();
        descriptionText.text = thisCard.getDescription();
    }
}