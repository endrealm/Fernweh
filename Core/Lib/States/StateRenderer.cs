using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using Core.Scenes.Ingame.Views;
using Core.Utils;
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
    private readonly bool _clearRender;
    private readonly Queue<IChatComponent> _components = new();
    private readonly List<LabelSettings> _labelSettings = new();

    public StateRenderer(ILocalizationManager localizationManager, Language language, IFontManager font,
        Action<Color> changeBackgroundColor, bool clearRender)
    {
        _localizationManager = localizationManager;
        _language = language;
        _font = font;
        _changeBackgroundColor = changeBackgroundColor;
        _clearRender = clearRender;
    }

    public bool ClearRender => _clearRender;
    
    public void AddLabel(int x, int y, string key, LuaTable rawReplacements = null)
    {
        var replacements = LuaUtils.ReadReplacements(rawReplacements);
        var text = _localizationManager.GetData(key, replacements);

        _labelSettings.Add(new LabelSettings(x, y, text));
    }
    
    public void AddText(string key, LuaFunction callback = null, LuaTable rawReplacements = null)
    {
        var replacements = LuaUtils.ReadReplacements(rawReplacements);
        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimated(_font.GetChatFont(), () => callback?.Call());

        _components.Enqueue(text);
    }

    public void AddAction(LuaFunction callback, string key, LuaTable rawReplacements = null)
    {
        var replacements = LuaUtils.ReadReplacements(rawReplacements);

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

    public List<LabelSettings> GetLabelSettings()
    {
        return _labelSettings;
    }

    public void SetBackgroundColor(Color color)
    {
        _changeBackgroundColor.Invoke(color);
    }

}