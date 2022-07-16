using System;
using System.Collections.Generic;
using System.Text;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core.Input
{
    internal class MouseSnapshot : IUpdate<TopLevelUpdateContext>
    {
        public enum Directions
        {
            Left,  
            Right,
            Up,
            Down,
        }

        public MouseState CurrentKeyState;
        private MouseState _previousKeyState;

        private Vector2 origin;

        public void Update(float deltaTime, TopLevelUpdateContext context)
        {
            _previousKeyState = CurrentKeyState;
            CurrentKeyState = Mouse.GetState();

            int w = (int)context.Camera.BoundingRectangle.Width * 2;
            int h = (int)context.Camera.BoundingRectangle.Height * 2;
            origin = new Vector2(w - h / 2, h / 2);
        }

        public bool MouseUpInDirection(Directions dir)
        {

            var direction = origin - CurrentKeyState.Position.ToVector2();
            if (direction.X > origin.Y) return false; // cant click on chat box

            if(CurrentKeyState.LeftButton == ButtonState.Pressed && _previousKeyState.LeftButton == ButtonState.Released)
            {
                if(dir == Directions.Up && direction.Y > 0 && direction.Y > Math.Abs(direction.X) ||
                    dir == Directions.Down && direction.Y < 0 && direction.Y * -1 > Math.Abs(direction.X) ||
                    dir == Directions.Left && direction.X > 0 && direction.X > Math.Abs(direction.Y) ||
                    dir == Directions.Right && direction.X < 0 && direction.X * -1 > Math.Abs(direction.Y))
                    return true;
            }
            return false;
        }
    }
}
