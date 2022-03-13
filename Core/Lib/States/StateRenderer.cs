using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using PipelineExtensionLibrary;
// ReSharper disable InconsistentNaming

namespace Core.States;

public class StateRenderer
{
    private readonly DialogTranslationData _translationData;
    private readonly Language _language;
    private readonly SpriteFont _font;
    private readonly Queue<IChatComponent> _components = new();

    public StateRenderer(DialogTranslationData translationData, Language language, SpriteFont font)
    {
        _translationData = translationData;
        _language = language;
        _font = font;
    }
    public void addText(string key)
    {
        var text = _translationData
            .TranslationGroups[key]
            .TranslatedComponents[_language]
            .BuildAnimated(_font);
        _components.Enqueue(text);
    }
    
    public void addAction(LuaFunction callback, string key)
    {
        var text = _translationData
            .TranslationGroups[key]
            .TranslatedComponents[_language]
            .BuildAnimatedAction(_font, () => callback.Call());
        _components.Enqueue(text);
    }

    public Queue<IChatComponent> Build()
    {
        return _components;
    }
}