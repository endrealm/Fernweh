namespace Core.Scenes.Ingame.Battle;

public class Stats
{
    public int Strength { get; set; }
    public int Constitution { get; set; }
    public int Dexterity { get; set; }
    public int Intellect { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Armor { get; set; }

    public Stats Clone()
    {
        return new Stats()
        {
            Strength = Strength,
            Intellect = Intellect,
            Dexterity = Dexterity,
            Constitution = Constitution,
            Wisdom = Wisdom,
            Charisma = Charisma,
            Health = Health,
            Mana = Mana,
            Armor = Armor,
        };
    }
}