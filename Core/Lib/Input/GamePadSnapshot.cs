using Microsoft.Xna.Framework.Input;
using Core.Utils;

namespace Core.Input
{
    internal class GamePadSnapshot : IUpdate<TopLevelUpdateContext>
    {
        public GamePadState CurrentButtonState = GamePad.GetState(0);
        private GamePadState _previousButtonState = GamePad.GetState(0);

        public void Update(float deltaTime, TopLevelUpdateContext context)
        {
            _previousButtonState = CurrentButtonState;
            CurrentButtonState = GamePad.GetState(0);
        }

        public bool HasBeenPressed(Buttons[] buttons)
        {
            if (!CurrentButtonState.IsConnected) return false;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (CurrentButtonState.IsButtonDown(buttons[i]) && !_previousButtonState.IsButtonDown(buttons[i]))
                    return true;
            }
            return false;
        }

        public bool AnyButtonPress()
        {
            return CurrentButtonState != _previousButtonState;
        }
    }
}
