[System.Serializable]

public class MinionCard : Card
{
    private int health;
    private int attack;

    public MinionCard(int ID, string Name, int Cost, string Description, int health, int attack)
        : base(ID, Name, Cost, Description)
    {
        this.health = health;
        this.attack = attack;
    }

    // Public Getter Methods
    public int getHealth()
    {
        return health;
    }
    public int getAttack()
    {
        return attack;
    }
}