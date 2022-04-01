using System.Collections.Generic;
using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle.Impl;

public class StaticBattleSpriteManager: ILoadable, IBattleSpriteManager
{
    private Dictionary<string, Texture2D> _sprites = new();
    public void Load(ContentLoader content)
    {
        _sprites.Add("fallback", content.LoadTexture("Sprites/Battle/player.png"));
        
        // samples
        _sprites.Add("Geralt", content.LoadTexture("Sprites/Battle/player.png"));
        _sprites.Add("Ciri", content.LoadTexture("Sprites/Battle/player.png"));
        _sprites.Add("Yennifer", content.LoadTexture("Sprites/Battle/player.png"));
        _sprites.Add("Triss", content.LoadTexture("Sprites/Battle/player.png"));
        _sprites.Add("test", content.LoadTexture("Sprites/Battle/monster.png"));
    }

    public Texture2D GetTexture(string key)
    {
        if (_sprites.TryGetValue(key, out var sprite)) return sprite;
        return _sprites["fallback"];
    }
}