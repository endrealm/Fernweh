using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public abstract class BaseComponent: IChatComponent
{
    

    protected abstract float CalculateHeight();
    protected abstract float CalculateWidth();
    public abstract void Render(SpriteBatch spriteBatch, ChatRenderContext context);
    public Vector2 Dimensions => new(CalculateWidth(), CalculateHeight());
    public abstract float MaxWidth { get; set; }
    public abstract void SetOnDone(Action action);
    public abstract float Width { get; }
    public abstract void Update(float deltaTime, ChatUpdateContext context);
}