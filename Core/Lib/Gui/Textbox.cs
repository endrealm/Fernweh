using System.Runtime.CompilerServices;
using Apos.Input;
using Apos.Tweens;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Track = Apos.Input.Track;

namespace Core.Gui;
// TODO: Textbox shouldn't change it's size. It should take a "preferred width" and just use that.
//       Will need to add scrolling.

public class Textbox : Component
{
    protected FloatTween _blink = new(0f, 1f, 1500, Easing.Linear);
    protected int _cursor;
    protected RectangleF _cursorRect;

    protected int _fontSize;
    protected FloatTween _input = new(0f, 1f, 50, Easing.Linear);

    protected int _inputDelay = 50;
    protected int _inputInitialDelay = 400;
    protected string _placeholder = null!;

    protected bool _pressed;
    protected Vector2 _size;

    protected string _text = null!;

    public Textbox(int id, string text, string placeholder, int fontSize, Color c) : base(id)
    {
        Text = text;
        Placeholder = placeholder;
        FontSize = fontSize;
        Color = c;
    }

    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                _size = GuiHelper.MeasureString(_text, _fontSize);
                if (Cursor > _text.Length) Cursor = _text.Length;
            }
        }
    }

    public string Placeholder
    {
        get => _placeholder;
        set
        {
            if (_placeholder != value) _placeholder = value;
        }
    }

    public int Padding { get; set; } = 10;
    public Color Color { get; set; }

    public int FontSize
    {
        get => _fontSize;
        set
        {
            // TODO: Only change size on next loop since it changes the layout.
            if (value != _fontSize)
            {
                _fontSize = value;
                _size = GuiHelper.MeasureString(_text, _fontSize);
            }
        }
    }

    public override bool IsFocused
    {
        get => base.IsFocused;
        set
        {
            base.IsFocused = value;
            _blink.StartTime = TweenHelper.TotalMS;
        }
    }

    public override bool IsFocusable { get; set; } = true;

    protected int Cursor
    {
        get => _cursor;
        set
        {
            if (value >= 0 && value <= _text.Length && _cursor != value)
            {
                _cursor = value;
                _blink.StartTime = TweenHelper.TotalMS;
            }
        }
    }

    public override void UpdatePrefSize(GameTime gameTime)
    {
        PrefWidth = MathHelper.Max(_size.X, 100) + Padding * 2;
        PrefHeight = _size.Y + Padding * 2;
    }

    public override void UpdateInput(GameTime gameTime)
    {
        if (Clip.Contains(GuiHelper.Mouse) && Default.MouseInteraction.Pressed())
        {
            _pressed = true;
            Cursor = MouseToCursor(GuiHelper.Mouse.X, _text);
            GrabFocus(this);
        }

        if (IsFocused)
        {
            if (_pressed && Default.MouseInteraction.HeldOnly()) Cursor = MouseToCursor(GuiHelper.Mouse.X, _text);
            if (_pressed && Default.MouseInteraction.Released())
            {
                _pressed = false;
                Cursor = MouseToCursor(GuiHelper.Mouse.X, _text);
            }

            MoveCursor(Default.MoveLeft, -1);
            MoveCursor(Default.MoveRight, 1);

            if (Default.MoveUp.Released()) Cursor = 0;
            if (Default.MoveDown.Released()) Cursor = _text.Length;

            foreach (var te in InputHelper.TextEvents)
            {
                if (te.Key == Keys.Tab) continue;

                if (te.Key == Keys.Enter)
                {
                }
                else if (te.Key == Keys.Back)
                {
                    if (Cursor > 0 && _text.Length > 0)
                    {
                        Cursor--;
                        Text = _text.Remove(Cursor, 1);
                    }
                }
                else if (te.Key == Keys.Delete)
                {
                    if (_text.Length > 0 && Cursor < _text.Length)
                    {
                        Text = _text.Remove(Cursor, 1);
                        _blink.StartTime = TweenHelper.TotalMS;
                    }
                }
                else
                {
                    Text = _text.Insert(Cursor, $"{te.Character}");
                    Cursor++;
                }

                Track.KeyboardCondition.Consume(te.Key);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (IsFocused)
            if (TweenHelper.TotalMS > _blink.StartTime + _blink.Duration)
                _blink.StartTime = TweenHelper.TotalMS;
    }

    public override void Draw(GameTime gameTime)
    {
        GuiHelper.PushScissor(Clip);
        var drawText = _text;

        if (_text.Length == 0) drawText = _placeholder;

        if (IsFocused)
        {
            GuiHelper.SpriteBatch.DrawRectangle(Bounds, Color.White, 2f);

            var alignLeft = Left + Padding;

            var cursorLeft = alignLeft;
            if (Cursor > 0 && Cursor <= drawText.Length)
                cursorLeft = alignLeft + GuiHelper.MeasureStringTight(drawText.Substring(0, Cursor), _fontSize).X;
            if (_blink.Value <= 0.5f)
                GuiHelper.SpriteBatch.FillRectangle(new RectangleF(cursorLeft, Top, 2f, Height), Color.White);
        }
        else
        {
            GuiHelper.SpriteBatch.DrawRectangle(Bounds, new Color(76, 76, 76), 2f);
        }

        var font = GuiHelper.GetFont(_fontSize);
        GuiHelper.SpriteBatch.DrawString(font, drawText, XY + new Vector2(Padding), Color, GuiHelper.FontScale);

        GuiHelper.PopScissor();
    }

    public static Textbox Put(ref string text, string placeholder = "", int fontSize = 30, Color? color = null,
        [CallerLineNumber] int id = 0, bool isAbsoluteId = false)
    {
        // 1. Check if Textbox with id already exists.
        //      a. If already exists. Get it.
        //      b  If not, create it.
        // 4. Ping it.
        id = GuiHelper.CurrentIMGUI.CreateId(id, isAbsoluteId);
        GuiHelper.CurrentIMGUI.TryGetValue(id, out var c);

        color ??= Color.White;

        Textbox a;
        if (c is Textbox)
        {
            a = (Textbox) c;
            if (a.IsFocused)
                text = a.Text;
            else
                a.Text = text;

            a.Placeholder = placeholder;
            a.Color = color.Value;
            a.FontSize = fontSize;
        }
        else
        {
            a = new Textbox(id, text, placeholder, fontSize, color.Value);
        }

        var parent = GuiHelper.CurrentIMGUI.GrabParent(a);

        if (a.LastPing != InputHelper.CurrentFrame)
        {
            a.LastPing = InputHelper.CurrentFrame;
            a.Index = parent.NextIndex();
        }

        return a;
    }

    protected void MoveCursor(ICondition condition, int direction)
    {
        if (condition.Pressed())
        {
            Cursor += direction;
            _input.StartTime = TweenHelper.TotalMS;
            _input.Duration = _inputInitialDelay;
        }

        if (condition.HeldOnly())
            if (TweenHelper.TotalMS > _input.StartTime + _input.Duration)
            {
                Cursor += direction;
                _input.StartTime = TweenHelper.TotalMS;
                _input.Duration = _inputDelay;
            }
    }

    protected int MouseToCursor(float x, string text)
    {
        var left = Left + Padding;
        var currentOffset = left;
        var currentPosition = 0;
        for (var i = 0; i < text.Length; i++)
        {
            if (x < currentOffset) break;
            currentPosition++;
            currentOffset = left + GuiHelper.MeasureStringTight(text.Substring(0, currentPosition), _fontSize).X;
        }

        return currentPosition;
    }
}