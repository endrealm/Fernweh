using Microsoft.Xna.Framework.Input;
using Core.Utils;

namespace Core.Input;

internal class KeyboardSnapshot: IUpdate<TopLevelUpdateContext>
{
    public KeyboardState CurrentKeyState;
    private KeyboardState _previousKeyState;

    public void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _previousKeyState = CurrentKeyState;
        CurrentKeyState = Keyboard.GetState();
    }

    public bool HasBeenPressed(Keys[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if(CurrentKeyState.IsKeyDown(keys[i]) && !_previousKeyState.IsKeyDown(keys[i]))
                return true;
        }
        return false;
    }
}