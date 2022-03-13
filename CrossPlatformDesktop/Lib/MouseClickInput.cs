using Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrossPlatformDesktop;

public class MouseClickInput: IUpdateableClickInput
{
    public bool ClickedThisFrame  { get; private set; }
    public Vector2 ScreenSpacedCoordinates  { get; private set; }

    private MouseState _lastState;
    public void Update(GameTime gameTime)
    {
        
        
        var current = Mouse.GetState();
        ScreenSpacedCoordinates = current.Position.ToVector2();
        if (current.LeftButton == ButtonState.Pressed && _lastState.LeftButton != ButtonState.Pressed)
        {
            ClickedThisFrame = true;
        }
        else
        {
            ClickedThisFrame = false;
        }
        _lastState = current;
    }
}