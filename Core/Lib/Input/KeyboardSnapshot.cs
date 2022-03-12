using Microsoft.Xna.Framework.Input;
using Core.Utils;

namespace Core.Input;

internal class KeyboardSnapshot: IUpdate<TopLeveUpdateContext>
{
    public KeyboardState currentKeyState;
    private KeyboardState _previousKeyState;

    public void Update(float deltaTime, TopLeveUpdateContext context)
    {
        _previousKeyState = currentKeyState;
        currentKeyState = Keyboard.GetState();
    }

    public bool HasBeenPressed(Keys[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if(currentKeyState.IsKeyDown(keys[i]) && !_previousKeyState.IsKeyDown(keys[i]))
                return true;
        }
        return false;
    }
}