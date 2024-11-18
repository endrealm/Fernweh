using Core.Utils;

namespace Core.Scenes.Ingame;

public class IngameUpdateContext : IUpdateContext
{
    public IngameUpdateContext(TopLevelUpdateContext topLevelUpdateContext)
    {
        TopLevelUpdateContext = topLevelUpdateContext;
    }

    public TopLevelUpdateContext TopLevelUpdateContext { get; }
}