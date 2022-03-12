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

        public void Update(float deltaTime, TopLeveUpdateContext context)
        {
            keyboardSnapshot.Update(deltaTime, context);
        }

        public static bool AnyInput()
        {
            return keyboardSnapshot.currentKeyState.GetPressedKeys().Length > 0;
        }

        public static bool MoveUp()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.W, Keys.Up });
        }

        public static bool MoveDown()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.S, Keys.Down });
        }

        public static bool MoveLeft()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.A, Keys.Left });
        }

        public static bool MoveRight()
        {
            return keyboardSnapshot.HasBeenPressed(new Keys[] { Keys.D, Keys.Right });
        }
    }
}
