namespace Core.Scenes.Ingame.Battle;

public class Stats
{
    public int Strength { get; set; }
    public int Intellect { get; set; }
    public int Agility { get; set; }
    public int Defense { get; set; }
    public int Spirit { get; set; }
    public int Evasion { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }

    public Stats Clone()
    {
        return new Stats()
        {
            Strength = Strength,
            Intellect = Intellect,
            Agility = Agility,
            Defense = Defense,
            Spirit = Spirit,
            Evasion = Evasion,
            Health = Health,
            Mana = Mana,
        };
    }
}