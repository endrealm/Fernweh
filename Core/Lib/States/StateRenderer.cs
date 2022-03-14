using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Chat;

// ReSharper disable InconsistentNaming

namespace Core.States;

public class StateRenderer
{
    private readonly DialogTranslationData _translationData;
    private readonly Language _language;
    private readonly IFontManager _font;
    private readonly Queue<IChatComponent> _components = new();

    public StateRenderer(DialogTranslationData translationData, Language language, IFontManager font)
    {
        _translationData = translationData;
        _language = language;
        _font = font;
    }
    public void addText(string key)
    {
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(_language))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimated(_font.GetChatFont());
        } 
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[_language]
                .BuildAnimated(_font.GetChatFont());
        }
        _components.Enqueue(text);
    }
    
    public void addAction(LuaFunction callback, string key)
    {
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(_language))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimatedAction(_font.GetChatFont(), () => callback.Call());
        } 
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[_language]
                .BuildAnimatedAction(_font.GetChatFont(), () => callback.Call());
        }
        _components.Enqueue(text);
    }

    public Queue<IChatComponent> Build()
    {
        return _components;
    }
}