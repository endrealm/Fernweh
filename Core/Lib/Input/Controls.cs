using Core.Utils;
using Microsoft.Xna.Framework.Input;

namespace Core.Input;

public class Controls : IUpdate<TopLevelUpdateContext>
{
    private static readonly KeyboardSnapshot _keyboardSnapshot = new();
    private static readonly GamePadSnapshot _gamePadSnapshot = new();
    private static readonly MouseSnapshot _mouseSnapshot = new();

    public void Update(float deltaTime, TopLevelUpdateContext context)
    {
        _keyboardSnapshot.Update(deltaTime, context);
        _gamePadSnapshot.Update(deltaTime, context);
        _mouseSnapshot.Update(deltaTime, context);
    }

    public static bool AnyInput()
    {
        return _keyboardSnapshot.CurrentKeyState.GetPressedKeys().Length > 0
               || Mouse.GetState().LeftButton == ButtonState.Pressed
               || _gamePadSnapshot.AnyButtonPress();
    }

    public static bool MoveUp()
    {
        return _keyboardSnapshot.HasBeenPressed(new[] {Keys.W, Keys.Up})
               || _gamePadSnapshot.HasBeenPressed(new[] {Buttons.DPadUp, Buttons.LeftThumbstickUp})
               || _mouseSnapshot.MouseUpInDirection(MouseSnapshot.Directions.Up);
    }

    public static bool MoveDown()
    {
        return _keyboardSnapshot.HasBeenPressed(new[] {Keys.S, Keys.Down})
               || _gamePadSnapshot.HasBeenPressed(new[] {Buttons.DPadDown, Buttons.LeftThumbstickDown})
               || _mouseSnapshot.MouseUpInDirection(MouseSnapshot.Directions.Down);
    }

    public static bool MoveLeft()
    {
        return _keyboardSnapshot.HasBeenPressed(new[] {Keys.A, Keys.Left})
               || _gamePadSnapshot.HasBeenPressed(new[] {Buttons.DPadLeft, Buttons.LeftThumbstickLeft})
               || _mouseSnapshot.MouseUpInDirection(MouseSnapshot.Directions.Left);
    }

    public static bool MoveRight()
    {
        return _keyboardSnapshot.HasBeenPressed(new[] {Keys.D, Keys.Right})
               || _gamePadSnapshot.HasBeenPressed(new[] {Buttons.DPadRight, Buttons.LeftThumbstickRight})
               || _mouseSnapshot.MouseUpInDirection(MouseSnapshot.Directions.Right);
    }
}