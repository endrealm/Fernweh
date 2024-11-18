using Core.Utils;
using Microsoft.Xna.Framework.Input;

namespace Core.Input;

internal class KeyboardSnapshot : IUpdate<TopLevelUpdateContext>
{
    private KeyboardState _previousKeyState;
    public KeyboardState CurrentKeyState;

    public void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _previousKeyState = CurrentKeyState;
        CurrentKeyState = Keyboard.GetState();
    }

    public bool HasBeenPressed(Keys[] keys)
    {
        for (var i = 0; i < keys.Length; i++)
            if (CurrentKeyState.IsKeyDown(keys[i]) && !_previousKeyState.IsKeyDown(keys[i]))
                return true;
        return false;
    }
}