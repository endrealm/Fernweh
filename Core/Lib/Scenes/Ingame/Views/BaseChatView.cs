using System;
using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using PipelineExtensionLibrary;
using PipelineExtensionLibrary.Chat;

namespace Core.Scenes.Ingame.Views;

public class BaseChatView : IChatView
{
    protected Queue<IChatComponent> QueuedComponents = new();
    protected readonly List<IChatComponent> RunningComponents = new();
    protected int Width;
    private readonly DialogTranslationData _translationData;
    private readonly IFontManager _fontManager;

    public BaseChatView(DialogTranslationData translationData, IFontManager fontManager)
    {
        _translationData = translationData;
        _fontManager = fontManager;
    }

    private const int XMargin = 5;

    public void Load(ContentManager content)
    {
    }

    protected void Clear()
    {
        QueuedComponents = new Queue<IChatComponent>();
        RunningComponents.Clear();
        Width = 0;
    }

    [CanBeNull]
    protected IChatComponent LoadNextComponentInQueue()
    {
        if(QueuedComponents.Count == 0) return null;
        var item = QueuedComponents.Dequeue();
        RunningComponents.Add(item);
        item.SetOnDone(() => LoadNextComponentInQueue());
        return item;
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // Do width auto resize
        if (context.ChatWidth != Width)
        {
            Width = context.ChatWidth;
            RunningComponents.ForEach(component =>
            {
                component.MaxWidth = Width - XMargin * 2;
            });
        }
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        var offsetY = 0f;
        RunningComponents.ForEach(component =>
        {
            component.Render(spriteBatch, new ChatRenderContext(new Vector2(XMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        var tempRunningComponents = new List<IChatComponent>(RunningComponents);
        var offsetY = 0f;
        tempRunningComponents.ForEach(component =>
        {
            component.Update(deltaTime, new ChatUpdateContext(context, new Vector2(XMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }
    
    public void AddText(string key, Action callback = null)
    {
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(Language.EN_US))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimated(_fontManager.GetChatFont(), () => callback?.Invoke());
        }
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[Language.EN_US]
                .BuildAnimated(_fontManager.GetChatFont(), () => callback?.Invoke());
        }
        QueuedComponents.Enqueue(text);
    }

    public void AddAction(string key, Action callback)
    {
        var groups = _translationData.TranslationGroups;
        IChatComponent text;
        if (!groups.ContainsKey(key) || !groups[key].TranslatedComponents.ContainsKey(Language.EN_US))
        {
            // Fallback to key
            text = new ChatCompoundData(new List<IChatComponentData>()
            {
                new ChatTextData(Color.Red, key)
            }).BuildAnimatedAction(_fontManager.GetChatFont(), callback);
        }
        else
        {
            // select actual translation
            text = groups[key].TranslatedComponents[Language.EN_US]
                .BuildAnimatedAction(_fontManager.GetChatFont(), callback);
        }
        QueuedComponents.Enqueue(text);
    }
    
}