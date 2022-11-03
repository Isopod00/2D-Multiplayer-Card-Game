using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

// This class is meant to represent a single card
public class ThisCard : NetworkBehaviour
{
    [SyncVar]
    public int cardID;

    private Card thisCard;

    private bool initializedValues = false;

    // These are the Text fields for card GameObjects
    private Text nameText;
    private Text costText;
    private Text descriptionText;

    private Text textSlot1;
    private Text textSlot2;

    // Start is called before the first frame update
    void Start()
    {
        thisCard = CardDataBase.cardList[cardID];
    }

    // Update the card Text fields
    void Update()
    {
        if (!initializedValues)
        {
            Text[] textFields = gameObject.GetComponentsInChildren<Text>();

            if (textFields.Length > 0)
            {
                nameText = textFields[0];
                descriptionText = textFields[1];
                costText = textFields[2];

                textSlot1 = textFields[4];
                textSlot2 = textFields[3];

                initializedValues = true;
            }
        }

        if (initializedValues && thisCard is MinionCard)
        {
            MinionCard card = thisCard as MinionCard;
            textSlot1.text = card.getHealth().ToString();
            textSlot2.text = card.getAttack().ToString();

            nameText.text = card.getName();
            costText.text = card.getCost().ToString();
            descriptionText.text = card.getDescription();
        }
        else if (initializedValues && thisCard is BuildingCard)
        {
            BuildingCard card = thisCard as BuildingCard;
            textSlot1.text = card.getDefense().ToString();
            textSlot2.text = card.getIncome().ToString();

            nameText.text = card.getName();
            costText.text = card.getCost().ToString();
            descriptionText.text = card.getDescription();
        }
    }
}