using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Core.Scenes.Ingame;
using Core.Utils;

namespace Core.Input
{
    public class Controls: IUpdate<TopLeveUpdateContext>
    {
        private static KeyboardSnapshot keyboardSnapshot = new KeyboardSnapshot();
        private static GamePadSnapshot gamePadSnapshot = new GamePadSnapshot();

        public void Update(float deltaTime, TopLeveUpdateContext context)
        {
            keyboardSnapshot.Update(deltaTime, context);
            gamePadSnapshot.Update(deltaTime, context);
        }

        public static bool AnyInput()
        {
            return keyboardSnapshot.currentKeyState.GetPressedKeys().Length > 0
                || gamePadSnapshot.AnyButtonPress();
        }

        public static bool MoveUp()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.W, Keys.Up })
                || gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp });
        }

        public static bool MoveDown()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.S, Keys.Down })
                || gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown });
        }

        public static bool MoveLeft()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.A, Keys.Left })
                || gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft });
        }

        public static bool MoveRight()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.D, Keys.Right })
                || gamePadSnapshot.HasBeenPressed(new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight });
        }
    }
}
