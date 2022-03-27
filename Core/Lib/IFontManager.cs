using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public interface IFontManager
{

    void Load(ContentManager contentManager);
    SpriteFont GetChatFont();
}

public class SimpleFontManager : IFontManager
{
    
    private SpriteFont _font;

    public void Load(ContentManager content)
    {
        _font = content.Load<SpriteFont>("Fonts/TinyUnicode");
    }

    public SpriteFont GetChatFont()
    {
        return _font;
    }
}