
using Core.Content;
using Core.Scenes.Ingame.Localization;
using Core.UI;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public abstract class Scene: IRenderer<TopLevelRenderContext>, IUpdate<TopLevelUpdateContext>, ILoadable
{
    public IFontManager FontManager { get; }
    public ILocalizationManager RootLocalizationManager { get; }
    public ISceneManager SceneManager { get; protected set; }
    protected UiLayer _uiLayer;

    public Scene(IFontManager fontManager, ILocalizationManager rootLocalizationManager)
    {
        FontManager = fontManager;
        RootLocalizationManager = rootLocalizationManager;
        _uiLayer = new UiLayer(fontManager, rootLocalizationManager);
    }
    public virtual void InjectSceneManager(ISceneManager sceneManager)
    {
        SceneManager = sceneManager;
    }
    public virtual void Render(SpriteBatch spriteBatch, TopLevelRenderContext context) {}
    public virtual void Update(float deltaTime, TopLevelUpdateContext context) {}
    public virtual void Load(ContentLoader content) {}

    public virtual UiLayer UiLayer => _uiLayer;
    public virtual void Unload() {}
}