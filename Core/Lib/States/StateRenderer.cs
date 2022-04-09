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

    public StateRenderer(DialogTranslationData translationData, Language language, IFontManager font,
        Action<Color> changeBackgroundColor)
    {
        _translationData = translationData;
        _language = language;
        _font = font;
        _changeBackgroundColor = changeBackgroundColor;
    }

    public void AddText(string key, LuaFunction callback = null, LuaTable rawReplacements = null)
    {
        var replacements = ReadReplacements(rawReplacements);
        
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

    public void AddAction(LuaFunction callback, string key, LuaTable rawReplacements = null)
    {
        var replacements = ReadReplacements(rawReplacements);

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

    private Replacement[] ReadReplacements(LuaTable rawReplacements)
    {
        var list = new List<Replacement>();
        if (rawReplacements != null)
        {
            foreach (var replacement in rawReplacements.Values)
            {
                if (!(replacement is LuaTable table)) throw new Exception("Invalid replacement parameter");
                var values = table.Values.GetEnumerator();
                values.MoveNext();
                var key = values.Current;
                values.MoveNext();
                var value = values.Current;

                list.Add(new Replacement(key.ToString(), value.ToString()));
            }
        }

        return list.ToArray();
    }

}