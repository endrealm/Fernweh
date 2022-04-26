using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using Microsoft.Xna.Framework;
using NLua;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.States;

public class StateRenderer
{
    private readonly ILocalizationManager _localizationManager;
    private readonly Language _language;
    private readonly IFontManager _font;
    private readonly Action<Color> _changeBackgroundColor;
    private readonly Queue<IChatComponent> _components = new();

    public StateRenderer(ILocalizationManager localizationManager, Language language, IFontManager font,
        Action<Color> changeBackgroundColor)
    {
        _localizationManager = localizationManager;
        _language = language;
        _font = font;
        _changeBackgroundColor = changeBackgroundColor;
    }

    public void AddText(string key, LuaFunction callback = null, LuaTable rawReplacements = null)
    {
        var replacements = ReadReplacements(rawReplacements);
        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimated(_font.GetChatFont(), () => callback?.Call());

        _components.Enqueue(text);
    }

    public void AddAction(LuaFunction callback, string key, LuaTable rawReplacements = null)
    {
        var replacements = ReadReplacements(rawReplacements);

        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimatedAction(_font.GetChatFont(), () => callback.Call());
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

    private IReplacement[] ReadReplacements(LuaTable rawReplacements)
    {
        var list = new List<IReplacement>();
        if (rawReplacements == null) return list.ToArray();
        foreach (var replacement in rawReplacements.Values)
        {
            if (!(replacement is LuaTable table)) throw new Exception("Invalid replacement parameter");
            var values = table.Values.GetEnumerator();
            values.MoveNext();
            var key = values.Current;
            values.MoveNext();
            var value = values.Current;

            if (value is ChatWrapper wrapper)
            {
                list.Add(new WrapperReplacement(key!.ToString(), wrapper));
            }
            else
            {
                list.Add(new TextReplacement(key!.ToString(), value!.ToString()));
            }
        }

        return list.ToArray();
    }

}