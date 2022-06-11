using System;
using System.Collections.Generic;
using System.Linq;
using Core.Content;
using Core.Input;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Views;

public class BaseChatView : IChatView
{
    protected Queue<IChatComponent> QueuedComponents = new();
    protected readonly List<IChatComponent> RunningComponents = new();
    protected readonly List<ILabel> Labels = new();
    protected int Width;
    private readonly ILocalizationManager _localizationManager;
    private readonly IFontManager _fontManager;
    private float _lastFrameHeight = 0;
    private float _scrollOffset = 0;
    private bool _sticky = true;

    public BaseChatView(ILocalizationManager localizationManager, IFontManager fontManager)
    {
        _localizationManager = localizationManager;
        _fontManager = fontManager;
    }

    private const int XMargin = 5;

    public void Load(ContentLoader content)
    {
    }

    protected void Clear(int keepCount)
    {
        Labels.Clear();
        QueuedComponents = new Queue<IChatComponent>();
        var remaining = RunningComponents.Take(keepCount).ToList();
        RunningComponents.Clear();
        RunningComponents.AddRange(remaining);
        Width = 0;
        _scrollOffset = 0;
        _sticky = true;
    }
    
    public void Clear()
    {
        Labels.Clear();
        QueuedComponents = new Queue<IChatComponent>();
        RunningComponents.Clear();
        Width = 0;
        _scrollOffset = 0;
        _sticky = true;
    }

    public void SetSticky(bool sticky)
    {
        _sticky = sticky;
    }

    public ILabel DrawLabel(LabelSettings settings)
    {
        var label = new BasicLabel(settings, _fontManager.GetChatFont());
        Labels.Add(label);
        return label;
    }

    [CanBeNull]
    protected IChatComponent LoadNextComponentInQueue()
    {
        if(QueuedComponents.Count == 0) return null;
        var item = QueuedComponents.Dequeue();
        RunningComponents.Add(item);
        item.SetOnDone(() => LoadNextComponentInQueue());
        item.MaxWidth = Width - XMargin * 2;
        return item;
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        var screenHeight = context.BaseScreenSize.Y;
        // Do width auto resize
        if (context.ChatWidth != Width)
        {
            Width = context.ChatWidth;
            RunningComponents.ForEach(component =>
            {
                component.MaxWidth = Width - XMargin * 2;
            });
        }

        if (_scrollOffset < 0)
        {
            _scrollOffset = 0;
        }

        // Reset offset if chat has become smaller
        if (_sticky || _scrollOffset + screenHeight > _lastFrameHeight)
        {
            if (_scrollOffset != 0)
            {
                _sticky = true;
            }
            _scrollOffset = Math.Max(_lastFrameHeight - screenHeight, 0);
        }
        
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        var offsetY = 0f + -_scrollOffset;
        RunningComponents.ForEach(component =>
        {
            component.Render(spriteBatch, new ChatRenderContext(new Vector2(XMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
        
        Labels.ForEach(label => label.Render(spriteBatch, new LabelRenderContext(context.BaseScreenSize, context.ChatWidth)));
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        var tempRunningComponents = new List<IChatComponent>(RunningComponents);
        var offsetY = 0f + -_scrollOffset;
        var clicked = InteractionHelper.CursorHandled;
        tempRunningComponents.ForEach(component =>
        {
            var ctx = new ChatUpdateContext(context, new Vector2(XMargin, offsetY), clicked);
            component.Update(deltaTime, ctx);
            clicked = ctx.ClickHandled;
            offsetY += component.Dimensions.Y;
        });

        InteractionHelper.CursorHandled = clicked;

        _lastFrameHeight = offsetY + _scrollOffset;
        
        if (_sticky && context.TopLevelUpdateContext.ClickInput.ScrollWheelValue != 0) _sticky = false;

        _scrollOffset += context.TopLevelUpdateContext.ClickInput.ScrollWheelValue*-.05f;
    }
    
    public IChatComponent AddText(string key, Action callback = null, params IReplacement[] replacements)
    {
        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimated(_fontManager.GetChatFont(), () => callback?.Invoke());
        QueuedComponents.Enqueue(text);
        return text;
    }
    
    public IChatComponent AddText(string key, params IReplacement[] replacements)
    {
        return AddText(key, null, replacements);
    }

    public IChatComponent AddAction(string key, Action callback, params IReplacement[] replacements)
    {
        
        var text = _localizationManager.GetData(key, replacements)
            .Compile()
            .BuildAnimatedAction(_fontManager.GetChatFont(), callback);
        
        QueuedComponents.Enqueue(text);
        return text;
    }

    public void ForceLoadNext()
    {
        LoadNextComponentInQueue();
    }
}