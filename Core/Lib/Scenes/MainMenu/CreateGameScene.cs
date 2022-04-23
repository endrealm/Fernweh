using Core.Gui;
using Core.Scenes.Ingame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.MainMenu;

public class CreateGameScene: Scene
{
    
    private IFontManager _fontManager;
    private string _gameName = "";
    public CreateGameScene(IFontManager fontManager)
    {
        _fontManager = fontManager;
    }

    public override void Update(float deltaTime, TopLevelUpdateContext context)
    {
        if (Button.Put("Back").Clicked) {
            SceneManager.LoadScene(new CreateOrLoadScene(_fontManager));
        }
        MenuPanel.Push();
        Label.Put("Create new game?");
        var old = _gameName;
        var box = Textbox.Put(ref _gameName, "New Game");
        
        if (_gameName.Length > 16)
        {
            _gameName = old;
            box.Text = old;
        }
        
        Horizontal.Push();
        Label.Put("Save Name:");
        Label.Put(BuildProdName(), color: Color.Wheat);
        Horizontal.Pop();
        if (Button.Put("Start Game", color: IsValidName() ? Color.White : Color.Gray).Clicked && IsValidName()) {
            SceneManager.LoadScene(new IngameScene(_fontManager));
        }
        MenuPanel.Pop();
    }

    private bool IsValidName()
    {
        return BuildProdName().Length > 0;
    }

    private string BuildProdName()
    {
        return _gameName.Trim().ToLower().Replace((char)32, '_');
    }


    public override void Render(SpriteBatch spriteBatch, TopLevelRenderContext context)
    {
        context.GraphicsDevice.Clear(Color.CornflowerBlue);
    }
}