﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Chat;

public enum CompoundLayoutRule
{
    TopLeft,
    Center
}

public class CompoundTextComponent : BaseComponent, IChatInlineComponent, IChatContainerComponent
{
    private readonly List<IChatInlineComponent> _components;
    private readonly List<Action> _onDone = new();
    protected readonly CompoundLayoutRule Layout;
    private float _calculatedHeight;
    private float _calculatedWidth;
    private float _firstLineOffset;
    private float _maxContentWidth = -1;
    private float _maxHeight = -1;
    private float _maxWidth;
    private IShape _shape = new CompoundShape(new List<IShape>());

    public CompoundTextComponent(
        List<IChatInlineComponent> components,
        float width = -1,
        float maxWidth = -1,
        CompoundLayoutRule layout = CompoundLayoutRule.TopLeft
    ) : this(component => components, width, maxWidth, layout)
    {
    }

    public CompoundTextComponent(
        Func<CompoundTextComponent, List<IChatInlineComponent>> construct,
        float width = -1,
        float maxWidth = -1,
        CompoundLayoutRule layout = CompoundLayoutRule.TopLeft
    )
    {
        Layout = layout;
        _components = new List<IChatInlineComponent>();
        Width = width;
        MaxWidth = maxWidth;
        _components = construct.Invoke(this);
        Recalculate();
    }

    public override float Width { get; }

    public float MaxContentWidth
    {
        get => _maxContentWidth;
        set
        {
            _maxContentWidth = value;
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

    public override void Render(SpriteBatch spriteBatch, ChatRenderContext context)
    {
        var yOffset = 0f;
        var xOffset = 0f;

        if (Layout == CompoundLayoutRule.Center)
        {
            if (MaxWidth > _calculatedWidth) xOffset = (MaxWidth - _calculatedWidth) / 2f;

            if (MaxHeight > _calculatedHeight) yOffset = (MaxHeight - _calculatedHeight) / 2f;
        }

        foreach (var component in _components)
        {
            component.Render(spriteBatch, new ChatRenderContext(context.Position + new Vector2(xOffset, yOffset)));
            yOffset += component.Dimensions.Y - component.LastLineHeight;
        }

        // Debug text shape
        // Shape.WithOffset(context.Position + new Vector2(xOffset, 0)).DebugDraw(spriteBatch, Color.Red);
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

    public override void SetOnDone(Action action)
    {
        _onDone.Add(action);
    }

    public override void Update(float deltaTime, ChatUpdateContext context)
    {
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.Update(deltaTime, context);
        }

        if (_components.Any(component => component.DirtyContent))
        {
            Recalculate();
            DirtyContent = true;
        }
    }


    public float LastLineRemainingSpace => _components.LastOrDefault()?.LastLineRemainingSpace ?? Width;

    public float LastLength => _components.LastOrDefault()?.LastLength ?? 0;

    public float LastLineHeight => _components.LastOrDefault()?.LastLineHeight ?? 0;

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

    public bool EmptyLineEnd => _components.LastOrDefault()?.EmptyLineEnd ?? true;

    protected override float CalculateHeight()
    {
        return _calculatedHeight;
    }

    protected override float CalculateWidth()
    {
        return _calculatedWidth;
    }

    private void Recalculate()
    {
        var lineOffset = _firstLineOffset;
        var newCalcWidth = 0f;
        var newCalcHeight = 0f;
        var xOffset = 0f;
        var shapeList = new List<IShape>();
        for (var index = 0; index < _components.Count; index++)
        {
            var component = _components[index];
            component.MaxWidth = GetCorrectedContentWidth();
            component.FirstLineOffset = lineOffset;
            lineOffset = component.LastLength;
            component.DirtyContent = false;
            var (width, height) = component.Dimensions;
            shapeList.Add(component.Shape.WithOffset(new Vector2(xOffset, newCalcHeight)));
            newCalcHeight += height;
            var isLastItem = index == _components.Count - 1;

            if (component.LastLength != 0 && !isLastItem) newCalcHeight -= component.LastLineHeight;
            if (isLastItem && component.EmptyLineEnd) newCalcHeight += component.LastLineHeight;

            if (width + lineOffset > newCalcWidth) newCalcWidth = width + lineOffset;
        }

        _calculatedWidth = MaxContentWidth > 0 ? Math.Min(newCalcWidth, MaxContentWidth) : newCalcWidth;
        _calculatedHeight = newCalcHeight;
        _shape = new CompoundShape(shapeList);
    }

    private float GetCorrectedContentWidth()
    {
        return MaxContentWidth > 0 ? Math.Min(MaxWidth, MaxContentWidth) : MaxWidth;
    }

    public void Done()
    {
        _onDone.ForEach(action => action.Invoke());
    }
}