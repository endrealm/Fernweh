using Core.Input;
using Core.Saving;
using Core.Scenes.Modding;
using Core.Utils;
using MonoGame.Extended;

namespace Core;

public class TopLevelUpdateContext: IUpdateContext
{
    public TopLevelUpdateContext(IClickInput clickInput, OrthographicCamera camera, ModLoader modLoader, ISaveGameManager saveGameManager)
    {
        ClickInput = clickInput;
        Camera = camera;
        ModLoader = modLoader;
        SaveGameManager = saveGameManager;
    }

    public IClickInput ClickInput { get; }
    public OrthographicCamera Camera { get; }
    public ModLoader ModLoader { get; }
    public ISaveGameManager SaveGameManager { get; }
}