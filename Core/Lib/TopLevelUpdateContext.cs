using Core.Input;
using Core.Utils;
using MonoGame.Extended;

namespace Core;

public class TopLevelUpdateContext: IUpdateContext
{
    public TopLevelUpdateContext(IClickInput clickInput, OrthographicCamera camera)
    {
        ClickInput = clickInput;
        Camera = camera;
    }

    public IClickInput ClickInput { get; }
    public OrthographicCamera Camera { get; }
}