using Microsoft.Xna.Framework.Input;
using Core.Utils;

namespace Core.Input
{
    internal class GamePadSnapshot : IUpdate<TopLevelUpdateContext>
    {
        public GamePadState currentButtonState = GamePad.GetState(0);
        private GamePadState _previousButtonState = GamePad.GetState(0);

        public void Update(float deltaTime, TopLevelUpdateContext context)
        {
            _previousButtonState = currentButtonState;
            currentButtonState = GamePad.GetState(0);
        }

        public bool HasBeenPressed(Buttons[] buttons)
        {
            if (!currentButtonState.IsConnected) return false;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (currentButtonState.IsButtonDown(buttons[i]) && !_previousButtonState.IsButtonDown(buttons[i]))
                    return true;
            }
            return false;
        }

        public bool AnyButtonPress()
        {
            return currentButtonState != _previousButtonState;
        }
    }
}
