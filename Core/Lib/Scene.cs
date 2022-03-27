
using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core;

public abstract class Scene: IRenderer<TopLevelRenderContext>, IUpdate<TopLevelUpdateContext>, ILoadable
{
    public ISceneManager SceneManager { get; protected set; }

    public virtual void InjectSceneManager(ISceneManager sceneManager)
    {
        SceneManager = sceneManager;
    }
    public virtual void Render(SpriteBatch spriteBatch, TopLevelRenderContext context) {}
    public virtual void Update(float deltaTime, TopLevelUpdateContext context) {}
    public virtual void Load(ContentLoader content) {}
    public virtual void Unload() {}
}