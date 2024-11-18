using System.Runtime.CompilerServices;
using Apos.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;

namespace Core.Gui;

public class Icon : Component
{
    public Icon(int id, TextureRegion2D region) : base(id)
    {
        Region = region;
    }

    public TextureRegion2D Region { get; set; }

    public override void UpdatePrefSize(GameTime gametime)
    {
        PrefWidth = Region.Width;
        PrefHeight = Region.Height;
    }

    public override void Draw(GameTime gameTime)
    {
        GuiHelper.PushScissor(Clip);

        var halfWidth = (int) (Width / 2);
        var iconHalfWidth = (int) (PrefWidth / 2);

        var halfHeight = (int) (Height / 2);
        var iconHalfHeight = (int) (PrefHeight / 2);

        var pos = new Vector2(Left + halfWidth - iconHalfWidth, Top + halfHeight - iconHalfHeight);

        GuiHelper.SpriteBatch.Draw(Region, pos, Color.White);

        GuiHelper.PopScissor();
    }

    public static Icon Put(TextureRegion2D region, [CallerLineNumber] int id = 0, bool isAbsoluteId = false)
    {
        // 1. Check if Icon with id already exists.
        //      a. If already exists. Get it.
        //      b  If not, create it.
        // 4. Ping it.
        id = GuiHelper.CurrentIMGUI.CreateId(id, isAbsoluteId);
        GuiHelper.CurrentIMGUI.TryGetValue(id, out var c);

        Icon a;
        if (c is Icon)
        {
            a = (Icon) c;
            a.Region = region;
        }
        else
        {
            a = new Icon(id, region);
        }

        var parent = GuiHelper.CurrentIMGUI.GrabParent(a);

        if (a.LastPing != InputHelper.CurrentFrame)
        {
            a.LastPing = InputHelper.CurrentFrame;
            a.Index = parent.NextIndex();
        }

        return a;
    }
}