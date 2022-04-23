using Core.Input;
using Core.Scenes.Modding;
using Core.Utils;
using MonoGame.Extended;

namespace Core;

public class TopLevelUpdateContext: IUpdateContext
{
    public TopLevelUpdateContext(IClickInput clickInput, OrthographicCamera camera, ModLoader modLoader)
    {
        ClickInput = clickInput;
        Camera = camera;
        ModLoader = modLoader;
    }

    public IClickInput ClickInput { get; }
    public OrthographicCamera Camera { get; }
    public ModLoader ModLoader { get; }
}