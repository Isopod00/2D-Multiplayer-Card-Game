using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;

// This class is meant to represent a single card
public class ThisCard : NetworkBehaviour
{
    [SyncVar]
    public int cardID;

    private Card thisCard;

    // These are the Text fields for card GameObjects
    public Text nameText;
    public Text costText;
    public Text healthText;
    public Text descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        thisCard = CardDataBase.cardList[cardID];

        if (hasAuthority)
        {
            Text[] textFields = new Text[4];
            textFields = gameObject.GetComponentsInChildren<Text>();

            nameText = textFields[0];
            descriptionText = textFields[1];
            costText = textFields[2];
            healthText = textFields[3];
        }
    }

    void Update()
    {
        // Update the card Text fields
        nameText.text = thisCard.getName();
        costText.text = thisCard.getCost().ToString();
        healthText.text = thisCard.getHealth().ToString();
        descriptionText.text = thisCard.getDescription();
    }
}