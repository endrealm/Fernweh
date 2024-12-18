using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Apos.Input;
using Apos.Tweens;
using Microsoft.Xna.Framework;
using MouseCondition = Apos.Input.MouseCondition;
using Track = Apos.Input.Track;

namespace Core.Gui;

public class Horizontal : Component, IParent
{
    protected List<IComponent> _children = new();
    protected List<IComponent> _childrenRenderOrder = new();

    protected int _nextChildIndex;

    protected float _offsetX;

    protected FloatTween _offsetXTween = new(0f, 0f, 0, Easing.ExpoOut);
    protected float _offsetY;
    protected FloatTween _offsetYTween = new(0f, 0f, 0, Easing.ExpoOut);

    public Horizontal(int id) : base(id)
    {
    }

    public float OffsetX
    {
        get => _offsetX;
        set => SetOffset(_offsetXTween, ClampOffsetX(value));
    }

    public float OffsetY
    {
        get => _offsetY;
        set => SetOffset(_offsetYTween, ClampOffsetY(value));
    }

    public float FullWidth { get; set; } = 100;
    public float FullHeight { get; set; } = 100;

    public Vector2 OffsetXY
    {
        get => new(OffsetX, OffsetY);
        set
        {
            OffsetX = value.X;
            OffsetY = value.Y;
        }
    }

    public Vector2 FullSize
    {
        get => new(FullWidth, FullHeight);
        set
        {
            FullWidth = value.X;
            FullHeight = value.Y;
        }
    }

    public float ScrollIncrement { get; set; } = 50f;
    public float ScrollSpeed { get; set; } = 0.25f;
    public long ScrollMaxDuration { get; set; } = 1000;

    public override void UpdatePrefSize(GameTime gameTime)
    {
        float maxWidth = 0;
        float maxHeight = 0;

        foreach (var c in _children)
        {
            c.UpdatePrefSize(gameTime);
            maxWidth += c.PrefWidth;
            maxHeight = MathHelper.Max(c.PrefHeight, maxHeight);
        }

        PrefWidth = maxWidth;
        PrefHeight = maxHeight;
    }

    public override void UpdateSetup(GameTime gameTime)
    {
        // TODO: Keep current focus in view if it's in view?

        if (_offsetXTween.B != ClampOffsetX(_offsetXTween.B)) SetOffset(_offsetXTween, ClampOffsetX(_offsetXTween.B));

        _offsetX = _offsetXTween.Value;
        _offsetY = _offsetYTween.Value;

        var maxWidth = Width;
        var maxHeight = Height;

        var currentX = 0f;
        foreach (var c in _children)
        {
            c.X = currentX + X + OffsetX;
            c.Y = Y + OffsetY;
            c.Width = c.PrefWidth;
            c.Height = MathHelper.Min(c.PrefHeight, Height);

            maxHeight = MathHelper.Max(c.Height, maxHeight);
            c.Clip = c.Bounds.Intersection(Clip);

            c.UpdateSetup(gameTime);

            currentX += c.Width;
        }

        FullWidth = MathHelper.Max(currentX, maxWidth);
        FullHeight = maxHeight;
    }

    public override void UpdateInput(GameTime gameTime)
    {
        for (var i = _childrenRenderOrder.Count - 1; i >= 0; i--) _childrenRenderOrder[i].UpdateInput(gameTime);

        // TODO: If we don't scroll, don't consume the scroll to bubble the event up. Allows recursive scrolling.
        if (Clip.Contains(GuiHelper.Mouse) && Track.MouseCondition.Scrolled())
            SetOffset(_offsetXTween,
                ClampOffsetX(_offsetXTween.B + Math.Sign(MouseCondition.ScrollDelta) * ScrollIncrement));

        // TODO: Consume clicks on the panel? Otherwise it's possible to click stuff under it.
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var c in _children)
            c.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        foreach (var c in _childrenRenderOrder)
            if (Clip.Intersects(c.Clip))
                c.Draw(gameTime);

        // TODO: Draw scrollbars if needed?
    }

    public virtual void Add(IComponent c)
    {
        c.Parent = this;
        _children.Insert(c.Index, c);

        // TODO: Optimize this?
        _childrenRenderOrder.Add(c);
        _childrenRenderOrder.Sort((a, b) =>
        {
            if (a.IsFloatable && b.IsFloatable) return 0;

            if (!a.IsFloatable && !b.IsFloatable) return a.Index.CompareTo(b.Index);

            if (a.IsFloatable) return 1;

            return -1;
        });
    }

    public virtual void Remove(IComponent c)
    {
        c.Parent = null;
        _children.Remove(c);
        _childrenRenderOrder.Remove(c);
    }

    public virtual void Reset()
    {
        _nextChildIndex = 0;
    }

    public virtual int NextIndex()
    {
        return _nextChildIndex++;
    }

    /// <summary>
    ///     If this component has a parent, it will ask the parent to return this component's previous neighbor.
    ///     If it has children, it will return the last one.
    ///     Otherwise it will return itself.
    /// </summary>
    public override IComponent GetPrev()
    {
        return Parent?.GetPrev(this) ?? (_children.Count > 0 ? _children.Last().GetLast() : this);
    }

    /// <summary>
    ///     If this component has children, it will return the first one.
    ///     If it has a parent it will ask the parent to return this component's next neighbor.
    ///     Otherwise, it will return itself.
    /// </summary>
    public override IComponent GetNext()
    {
        return _children.Count > 0 ? _children.First() : Parent?.GetNext(this) ?? this;
    }

    /// <summary>
    ///     If the child isn't the first one, it will return the child before it.
    ///     Otherwise it will return itself.
    /// </summary>
    public virtual IComponent GetPrev(IComponent c)
    {
        var index = c.Index - 1;
        return index >= 0 ? _children[index].GetLast() : this;
    }

    /// <summary>
    ///     If the child isn't the last one, it will return the child after it.
    ///     If it has a parent, it will ask the parent to return this component's next neighbor.
    ///     Otherwise it will return itself.
    /// </summary>
    public virtual IComponent GetNext(IComponent c)
    {
        var index = c.Index + 1;
        return index < _children.Count ? _children[index] : Parent?.GetNext(this) ?? this;
    }

    /// <summary>
    ///     Returns the last child in this component tree.
    /// </summary>
    public override IComponent GetLast()
    {
        return _children.Count > 0 ? _children.Last().GetLast() : this;
    }

    public virtual void SendToTop(IComponent c)
    {
        if (c.IsFloatable)
        {
            _childrenRenderOrder.Remove(c);
            _childrenRenderOrder.Add(c);
        }

        if (c.X < X)
        {
            var xDiff = X - c.X;
            var oDiff = _offsetXTween.B - _offsetX;
            SetOffset(_offsetXTween, ClampOffsetX(_offsetXTween.B + xDiff - oDiff));
        }

        if (c.Right > Right)
        {
            var xDiff = Right - c.Right;
            var oDiff = _offsetXTween.B - _offsetX;
            SetOffset(_offsetXTween, ClampOffsetX(_offsetXTween.B + xDiff - oDiff));
        }

        Parent?.SendToTop(this);
    }

    protected virtual float ClampOffsetX(float x)
    {
        return MathHelper.Min(MathHelper.Max(x, Width - FullWidth), 0f);
    }

    protected virtual float ClampOffsetY(float y)
    {
        return MathHelper.Min(MathHelper.Max(y, Height - FullHeight), 0f);
    }

    protected void SetOffset(FloatTween ft, float b)
    {
        var a = ft.Value;
        ft.StartTime = TweenHelper.TotalMS;
        ft.A = a;
        ft.B = b;
        ft.Duration = GetDuration(a, b, ScrollSpeed, ScrollMaxDuration);
    }

    protected long GetDuration(float a, float b, float speed, long maxDuration)
    {
        return (long) Math.Min(Math.Abs((b - a) / speed), maxDuration);
    }

    public static Horizontal Push([CallerLineNumber] int id = 0, bool isAbsoluteId = false)
    {
        // 1. Check if horizontal with id already exists.
        //      a. If already exists. Get it.
        //      b  If not, create it.
        // 3. Push it on the stack.
        // 4. Ping it.
        id = GuiHelper.CurrentIMGUI.CreateId(id, isAbsoluteId);
        GuiHelper.CurrentIMGUI.TryGetValue(id, out var c);

        Horizontal a;
        if (c is Horizontal)
            a = (Horizontal) c;
        else
            a = new Horizontal(id);

        var parent = GuiHelper.CurrentIMGUI.GrabParent(a);

        if (a.LastPing != InputHelper.CurrentFrame)
        {
            a.Reset();
            a.LastPing = InputHelper.CurrentFrame;
            a.Index = parent.NextIndex();
        }

        GuiHelper.CurrentIMGUI.Push(a);

        return a;
    }

    public static void Pop()
    {
        GuiHelper.CurrentIMGUI.Pop();
    }
}