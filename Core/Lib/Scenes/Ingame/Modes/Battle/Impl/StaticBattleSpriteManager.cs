﻿using System.Collections.Generic;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle.Impl;

public class StaticBattleSpriteManager: ILoadable, IBattleSpriteManager
{
    private Dictionary<string, Texture2D> _sprites = new();
    public void Load(ContentManager content)
    {
        _sprites.Add("fallback", content.Load<Texture2D>("Sprites/Battle/player"));
        
        // samples
        _sprites.Add("Geralt", content.Load<Texture2D>("Sprites/Battle/player"));
        _sprites.Add("Ciri", content.Load<Texture2D>("Sprites/Battle/player"));
        _sprites.Add("Yennifer", content.Load<Texture2D>("Sprites/Battle/player"));
        _sprites.Add("Triss", content.Load<Texture2D>("Sprites/Battle/player"));
        _sprites.Add("test", content.Load<Texture2D>("Sprites/Battle/monster"));
    }

    public Texture2D GetTexture(string key)
    {
        if (_sprites.TryGetValue(key, out var sprite)) return sprite;
        return _sprites["fallback"];
    }
}