[System.Serializable]

public class BuildingCard : Card
{
    private int defense;
    private int income;

    public BuildingCard(int ID, string Name, int Cost, string Description, int defense, int income) 
        : base(ID, Name, Cost, Description)
    {
        this.defense = defense;
        this.income = income;
    }

    // Public Getter Methods
    public int getDefense()
    {
        return defense;
    }
    public int getIncome()
    {
        return income;
    }
}