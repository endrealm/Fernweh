namespace Core.Scenes.Ingame.Battle;

public class AbilityConfig
{
    public AbilityConfig(string id, object data = null)
    {
        Data = data;
        Id = id;
    }

    public string Id { get; }

    public object Data { get; }
}