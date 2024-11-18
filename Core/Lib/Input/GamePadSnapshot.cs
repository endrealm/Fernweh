using Core.Utils;
using Microsoft.Xna.Framework.Input;

namespace Core.Input;

internal class GamePadSnapshot : IUpdate<TopLevelUpdateContext>
{
    private GamePadState _previousButtonState = GamePad.GetState(0);
    public GamePadState CurrentButtonState = GamePad.GetState(0);

    public void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _previousButtonState = CurrentButtonState;
        CurrentButtonState = GamePad.GetState(0);
    }

    public bool HasBeenPressed(Buttons[] buttons)
    {
        if (!CurrentButtonState.IsConnected) return false;

        for (var i = 0; i < buttons.Length; i++)
            if (CurrentButtonState.IsButtonDown(buttons[i]) && !_previousButtonState.IsButtonDown(buttons[i]))
                return true;
        return false;
    }

    public bool AnyButtonPress()
    {
        return CurrentButtonState != _previousButtonState;
    }
}