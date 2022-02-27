using Microsoft.Xna.Framework.Input;

namespace Core.Input;

public class KeyboardSnapshot
{
    static KeyboardState _currentKeyState;
    static KeyboardState _previousKeyState;

    public static KeyboardState GetState()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
        return _currentKeyState;
    }

    public static bool IsPressed(Keys key)
    {
        return _currentKeyState.IsKeyDown(key);
    }

    public static bool HasBeenPressed(Keys key)
    {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }
}