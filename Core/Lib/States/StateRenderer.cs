using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scenes.Ingame.Chat;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Chat;

namespace Core.States;

public class StateRenderer
{
    private readonly DialogTranslationData _translationData;
    private readonly Language _language;
    private readonly IFontManager _font;
    private readonly Action<Color> _changeBackgroundColor;
    private readonly Queue<IChatComponent> _components = new();

    public StateRenderer(DialogTranslationData translationData, Language language, IFontManager font, Action<Color> changeBackgroundColor)
    {
        _translationData = translationData;
        _language = language;
        _font = font;
        _changeBackgroundColor = changeBackgroundColor;
    }
    public void AddText(string key, LuaFunction callback = null, params LuaTable[] rawReplacements)
    {
        var replacements = rawReplacements.Select(table => new Replacement((string) table[0], table[1].ToString())).ToArray();
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(_language))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimated(_font.GetChatFont(), () => callback?.Call(), replacements);
        }
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[_language]
                .BuildAnimated(_font.GetChatFont(), () => callback?.Call(), replacements);
        }
        _components.Enqueue(text);
    }

    public void AddAction(LuaFunction callback, string key, params LuaTable[] rawReplacements)
    {
        var replacements = rawReplacements.Select(table => new Replacement((string) table[0], table[1].ToString())).ToArray();
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(_language))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimatedAction(_font.GetChatFont(), () => callback.Call(), replacements);
        }
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[_language]
                .BuildAnimatedAction(_font.GetChatFont(), () => callback.Call(), replacements);
        }
        _components.Enqueue(text);
    }

    public void SetBackgroundColor(string color)
    {
        SetBackgroundColor(color.ToColor());
    }

    public Queue<IChatComponent> Build()
    {
        return _components;
    }

    public void SetBackgroundColor(Color color)
    {
        _changeBackgroundColor.Invoke(color);
    }
}