using System.Collections.Generic;
using Core.Content;
using Core.Scripting;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle.Impl;

public class DynamicBattleSpriteManager: IBattleSpriteManager
{
    private Dictionary<string, Texture2D> _sprites = new();
    private ContentLoader _content;
    public void Load(ContentLoader content, IGlobalEventHandler eventHandler)
    {
        if(content == null) return;
        _content = content;
        _sprites.Add("fallback", content.Load<Texture2D>("Sprites/Battle/player.png"));
        eventHandler.EmitLoadBattleSprites(this);
    }

    public Texture2D GetTexture(string key)
    {
        return _sprites.TryGetValue(key, out var sprite) ? sprite : _sprites["fallback"];
    }

    public void LoadTexture(string id, NamespacedKey resource)
    {
        _sprites.Add(id, _content.Load<Texture2D>(resource.Value, resource.Key));
    }
}