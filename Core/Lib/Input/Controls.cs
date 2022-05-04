using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Core.Scenes.Ingame;
using Core.Utils;

namespace Core.Input
{
    public class Controls: IUpdate<TopLevelUpdateContext>
    {
        private static KeyboardSnapshot _keyboardSnapshot = new KeyboardSnapshot();
        private static GamePadSnapshot _gamePadSnapshot = new GamePadSnapshot();

        public void Update(float deltaTime, TopLevelUpdateContext context)
        {
            _keyboardSnapshot.Update(deltaTime, context);
            _gamePadSnapshot.Update(deltaTime, context);
        }

        public static bool AnyInput()
        {
            return _keyboardSnapshot.CurrentKeyState.GetPressedKeys().Length > 0
                || Mouse.GetState().LeftButton == ButtonState.Pressed
                || _gamePadSnapshot.AnyButtonPress();
        }

        public static bool MoveUp()
        {
            return _keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.W, Keys.Up })
                || _gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp });
        }

        public static bool MoveDown()
        {
            return _keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.S, Keys.Down })
                || _gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown });
        }

        public static bool MoveLeft()
        {
            return _keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.A, Keys.Left })
                || _gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
        }

        public static bool MoveRight()
        {
            return _keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.D, Keys.Right })
                || _gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight });
        }
    }
}
