using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.Views;

public class BaseChatView : IChatView
{
    protected Queue<IChatComponent> QueuedComponents = new();
    protected readonly List<IChatComponent> RunningComponents = new();
    protected int Width;

    private const int XMargin = 5;

    public void Load(ContentManager content)
    {
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
    
}