[System.Serializable]

// This class defines the basic template for a Card object
public class Card
{
    // Private Instance Data
    private int cardID;
    private string cardName;
    private int cardCost;
    private string cardDescription;

    // Constructor for a Card object
    public Card(int ID, string Name, int Cost, string Description)
    {
        this.cardID = ID;
        this.cardName = Name;
        this.cardCost = Cost;
        this.cardDescription = Description;
    }

    // Public Getter Methods
    public int getID()
    {
        return cardID;
    }
    public string getName()
    {
        return cardName;
    }
    public int getCost()
    {
        return cardCost;
    }
    public string getDescription()
    {
        return cardDescription;
    }
}