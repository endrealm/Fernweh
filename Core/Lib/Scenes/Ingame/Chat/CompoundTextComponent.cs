using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class CompoundTextComponent: BaseComponent
{
    private List<IChatInlineComponent> _components;
    private float _maxWidth;
    private float _calculatedWidth;
    private float _calculatedHeight;

    public CompoundTextComponent(
        List<IChatInlineComponent> components,
        float width = -1,
        float maxWidth = -1
    )
    {
        _components = new List<IChatInlineComponent>();
        Width = width;
        MaxWidth = maxWidth;
        _components = components;
        Recalculate();
    }

    protected override float CalculateHeight()
    {
        return _calculatedHeight;
    }

    protected override float CalculateWidth()
    {
        return _calculatedWidth;
    }

    public override void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        var yOffset = 0f;
        foreach (var component in _components)
        {
            component.Render(spriteBatch, new ChatRenderContext(context.Position + new Vector2(0, yOffset)));
            yOffset += component.Dimensions.Y - component.LastLineHeight;
        }
    }

    public override float MaxWidth
    {
        get => _maxWidth;
        set
        {
            _maxWidth = value;
            Recalculate();
        }
    }

    private void Recalculate()
    {
        var xOffset = 0f;
        foreach (var component in _components)
        {
            component.MaxWidth = MaxWidth;
            component.FirstLineOffset = xOffset;
            xOffset = component.LastLength;
            component.DirtyContent = false;
        }
    }

    public override float Width { get; }
    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        var wasDirty = false;
        _components.ForEach(component =>
        {
            component.Update(deltaTime, context);

            if (component.DirtyContent) wasDirty = true;
        });
        if(wasDirty) Recalculate();
    }
}