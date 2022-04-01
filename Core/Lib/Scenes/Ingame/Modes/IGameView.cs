using Core.Utils;

namespace Core.Scenes.Ingame.Modes;

public interface IGameView : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    public bool WorldSpacedCoordinates { get; }
}