using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle;

public interface ISpriteManager
{
    public Texture2D GetTexture(string key);
}