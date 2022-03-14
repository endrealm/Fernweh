using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Input;

public interface IClickInput
{
    public bool ClickedThisFrame { get; }
    public Vector2 ScreenSpacedCoordinates { get; }
}

public interface IUpdateableClickInput: IClickInput, IUpdate
{
}