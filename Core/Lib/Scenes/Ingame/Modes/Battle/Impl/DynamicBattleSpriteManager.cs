using System.Linq;
using Core.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Battle.Impl;

public class DynamicBattleSpriteManager : ISpriteManager
{
    //private Dictionary<string, Texture2D> _sprites = new();
    private readonly ContentRegistry _content;

    public DynamicBattleSpriteManager(ContentRegistry content)
    {
        if (content == null) return;
        _content = content;

        if (!_content.pngs.ContainsKey("fallback") && _content.pngs.Count > 0)
            _content.pngs.Add("fallback", _content.pngs[_content.pngs.First().Key]);

        //eventHandler.EmitLoadBattleSprites(this);
    }

    public Texture2D GetTexture(string key)
    {
        return _content.pngs.TryGetValue(key, out var sprite) ? sprite : _content.pngs["fallback"];
    }

    //public void LoadTexture(string id, NamespacedKey resource)
    //{
    //    _sprites.Add(id, _content.Load<Texture2D>(resource.Value, resource.Key));
    //}
}