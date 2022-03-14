using System.Collections.Generic;
using Core.Scenes.Ingame.Chat;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Scenes.Ingame;

public class ChatView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    private Queue<IChatComponent> _queuedComponents = new();
    private List<IChatComponent> _runningComponents = new();
    private int _width = 0;

    private int _xMargin = 5;

    public void Load(ContentManager content)
    {
    }

    private void LoadNextComponentInQueue()
    {
        if(_queuedComponents.Count == 0) return;
        var item = _queuedComponents.Dequeue();
        _runningComponents.Add(item);
        item.SetOnDone(LoadNextComponentInQueue);
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // Do width auto resize
        if (context.ChatWidth != _width)
        {
            _width = context.ChatWidth;
            _runningComponents.ForEach(component =>
            {
                component.MaxWidth = _width - _xMargin * 2;
            });
        }
        // Draw UI here
        spriteBatch.FillRectangle(new Vector2(), new Size2(context.ChatWidth, context.BaseScreenSize.Y), context.BackgroundColor);
        var offsetY = 0f;
        _runningComponents.ForEach(component =>
        {
            component.Render(spriteBatch, new ChatRenderContext(new Vector2(_xMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        var tempRunningComponents = new List<IChatComponent>(_runningComponents);
        var offsetY = 0f;
        tempRunningComponents.ForEach(component =>
        {
            component.Update(deltaTime, new ChatUpdateContext(context, new Vector2(_xMargin, offsetY)));
            offsetY += component.Dimensions.Y;
        });
    }

    public void RenderResults(StateRenderer renderer)
    {
        _width = 0; // reset width so rescale is triggered
        _queuedComponents = renderer.Build();
        _runningComponents.Clear();
        LoadNextComponentInQueue();
    }
}