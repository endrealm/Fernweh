using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class CompoundTextComponent: BaseComponent
{
    private List<IChatInlineComponent> _components;
    private float _maxWidth;
    private float _calculatedWidth;
    private float _calculatedHeight;
    private Action _onDone = () => {};

    public CompoundTextComponent(
        List<IChatInlineComponent> components,
        float width = -1,
        float maxWidth = -1
    ): this(component => components, width, maxWidth)
    {
        
    }
    
    public CompoundTextComponent(
        Func<CompoundTextComponent, List<IChatInlineComponent>> construct,
        float width = -1,
        float maxWidth = -1
    )
    {
        _components = new List<IChatInlineComponent>();
        Width = width;
        MaxWidth = maxWidth;
        _components = construct.Invoke(this);
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
        var newCalcWidth = 0f;
        var newCalcHeight = 0f;
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.MaxWidth = MaxWidth;
            component.FirstLineOffset = xOffset;
            xOffset = component.LastLength;
            component.DirtyContent = false;
            var (width, height) = component.Dimensions;
            newCalcHeight += height;

            if (component.LastLength != 0 && ( index != (_components.Count-1)))
            {
                newCalcHeight -= component.LastLineHeight;
            } 
            if (index == (_components.Count-1) && component.EmptyLineEnd)
            {
                newCalcHeight += component.LastLineHeight;
            }

            if (component.Dimensions.X > newCalcWidth)
            {
                newCalcWidth = width;
            }
        }

        _calculatedWidth = newCalcWidth;
        _calculatedHeight = newCalcHeight;
    }

    public override void SetOnDone(Action action)
    {
        this._onDone = action;
    }

    public override float Width { get; }
    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.Update(deltaTime, context);
        }

        if(_components.Any(component => component.DirtyContent)) Recalculate();
    }

    public void AppendComponents(List<IChatInlineComponent> chatInlineComponents)
    {
        _components.AddRange(chatInlineComponents);
        Recalculate();
    }
    
    public void AppendComponent(IChatInlineComponent chatInlineComponents)
    {
        AppendComponents(new List<IChatInlineComponent> {chatInlineComponents});
    }

    public void Done()
    {
        _onDone.Invoke();
    }
}