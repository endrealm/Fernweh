namespace Core.Scenes.Ingame.Battle;

public class AbilityConfig
{
    private readonly object _data;

    public AbilityConfig(string id, object data = null)
    {
        _data = data;
        Id = id;
    }

    public string Id { get; }

    public object Data => _data;
}