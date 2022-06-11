using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public class CompoundTextComponent: BaseComponent, IChatInlineComponent, IChatContainerComponent
{
    private List<IChatInlineComponent> _components;
    private float _maxWidth;
    private float _maxHeight = -1;
    private float _calculatedWidth;
    private float _calculatedHeight;
    private List<Action> _onDone = new List<Action>();
    private IShape _shape = new CompoundShape(new List<IShape>());
    private float _firstLineOffset;

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
        
        // Debug text shape
        // Shape.WithOffset(context.Position).DebugDraw(spriteBatch, Color.Red);
    }

    public override float MaxWidth
    {
        get => _maxWidth;
        set
        {
            _maxWidth = value;
            Recalculate();
            DirtyContent = true;
        }
    }

    public override IShape Shape => _shape;

    private void Recalculate()
    {
        var xOffset = _firstLineOffset;
        var newCalcWidth = 0f;
        var newCalcHeight = 0f;
        var shapeList = new List<IShape>();
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.MaxWidth = MaxWidth;
            component.FirstLineOffset = xOffset;
            xOffset = component.LastLength;
            component.DirtyContent = false;
            var (width, height) = component.Dimensions;
            shapeList.Add(component.Shape.WithOffset(new Vector2(0, newCalcHeight)));
            newCalcHeight += height;
            var isLastItem = index == (_components.Count - 1);

            if (component.LastLength != 0 && !isLastItem)
            {
                newCalcHeight -= component.LastLineHeight;
            } 
            if (isLastItem && component.EmptyLineEnd)
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
        _shape = new CompoundShape(shapeList);
    }

    public override void SetOnDone(Action action)
    {
        this._onDone.Add(action);
    }

    public override float Width { get; }
    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.Update(deltaTime, context);
        }

        if(_components.Any(component => component.DirtyContent))
        {
            Recalculate();
            DirtyContent = true;
        }
    }

    public float MaxHeight
    {
        get => _maxHeight;
        set
        {
            _maxHeight = value;
            Recalculate();
            DirtyContent = true;
        }
    }
    public void AppendComponents(List<IChatInlineComponent> chatInlineComponents)
    {
        _components.AddRange(chatInlineComponents);
        Recalculate();
        DirtyContent = true;
    }
    
    public void AppendComponent(IChatInlineComponent chatInlineComponents)
    {
        AppendComponents(new List<IChatInlineComponent> {chatInlineComponents});
    }

    public void Done()
    {
        _onDone.ForEach(action => action.Invoke());
    }


    public float LastLineRemainingSpace
    {
        get => _components.LastOrDefault()?.LastLineRemainingSpace ?? Width;
    }

    public float LastLength
    {
        get => _components.LastOrDefault()?.LastLength ?? 0;
    }
    public float LastLineHeight
    {
        get => _components.LastOrDefault()?.LastLineHeight ?? 0;
    }

    public float FirstLineOffset
    {
        get => _firstLineOffset;
        set
        {
            _firstLineOffset = value;
            Recalculate();
            DirtyContent = true;
        }
    }

    public bool DirtyContent { get; set; }

    public bool EmptyLineEnd
    {
        get => _components.LastOrDefault()?.EmptyLineEnd ?? true;
    }
}