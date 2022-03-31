using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle;

public interface IBattleSpriteManager
{
    public Texture2D GetTexture(string key);
}