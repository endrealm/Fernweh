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
    private readonly IFontManager _font;
    private readonly Action<Color> _changeBackgroundColor;
    private readonly bool _clearRender;
    private readonly Queue<IChatComponent> _components = new();
    private readonly List<LabelSettings> _labelSettings = new();
    private RenderMode _renderMode = RenderMode.Typewriter;

    public StateRenderer(ILocalizationManager localizationManager, IFontManager font,
        Action<Color> changeBackgroundColor, bool clearRender)
    {
        _localizationManager = localizationManager;
        _font = font;
        _changeBackgroundColor = changeBackgroundColor;
        _clearRender = clearRender;
    }

    public bool ClearRender => _clearRender;

    public StateRenderer SetMode(string mode)
    {
        _renderMode = StringToMode(mode);
        return this;
    }

    public void AddLabel(int x, int y, string key, LuaTable rawReplacements = null)
    {
        var replacements = LuaUtils.ReadReplacements(rawReplacements);
        var text = _localizationManager.GetData(key, replacements);

        _labelSettings.Add(new LabelSettings(x, y, text));
    }
    
    public void AddText(string key, LuaFunction callback = null, LuaTable rawReplacements = null)
    {
        if (key.Length == 0) key = " ";
        var replacements = LuaUtils.ReadReplacements(rawReplacements);
        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimated(_font.GetChatFont(), () => callback?.Call(), animated: _renderMode == RenderMode.Typewriter);

        _components.Enqueue(text);
    }

    public void AddAction(LuaFunction callback, string key, LuaTable rawReplacements = null)
    {
        var replacements = LuaUtils.ReadReplacements(rawReplacements);

        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimatedAction(_font.GetChatFont(), () => callback.Call(), animated: _renderMode == RenderMode.Typewriter);
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

    private RenderMode StringToMode(string value)
    {
        switch (value.ToLower())
        {
            case "static": return RenderMode.Static;
            case "typewriter": return RenderMode.Typewriter;
        }
        
        return RenderMode.Static;
    }
}

public enum RenderMode
{
    Static,
    Typewriter,
}