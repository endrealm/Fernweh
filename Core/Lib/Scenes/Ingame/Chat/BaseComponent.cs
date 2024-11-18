using System;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public abstract class BaseComponent : IChatComponent
{
    public abstract float Width { get; }
    public abstract void Render(SpriteBatch spriteBatch, ChatRenderContext context);
    public Vector2 Dimensions => new(CalculateWidth(), CalculateHeight());
    public abstract float MaxWidth { get; set; }
    public abstract IShape Shape { get; }
    public abstract void SetOnDone(Action action);
    public abstract void Update(float deltaTime, ChatUpdateContext context);


    protected abstract float CalculateHeight();
    protected abstract float CalculateWidth();
}