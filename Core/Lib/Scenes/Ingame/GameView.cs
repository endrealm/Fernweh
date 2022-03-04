using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;

namespace Core.Scenes.Ingame;

public class GameView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    MapData mapData = new MapData();
    WorldDataRegistry worldDataRegistry = new WorldDataRegistry();

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        //  find where the camera world pos is so we only render whats visible + 1 tile around for moving anims.
        var cameraGridPos = new Vector2((int)Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1, (int)Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);
        //System.Console.WriteLine(cameraGridPos);
        
        for (int x = (int)cameraGridPos.X; x < (int)cameraGridPos.X + 9; x++)
        {
            for (int y = (int)cameraGridPos.Y; y < (int)cameraGridPos.Y + 9; y++)
            {
                var tileName = mapData.GetTile(new Vector2(x, y));
        
                if (tileName !=  null)
                    spriteBatch.Draw(
                        worldDataRegistry.GetTile(tileName)._sprite,
                        new Rectangle((int)x * 32 + context.ChatWidth, y * 32, 32, 32),
                        Color.White);
            }
        }

        // test moving the camera
        context.TopLevelContext.Camera.Move(new Vector2(8, 8));
    }

    public void Load(ContentManager content)
    {
        worldDataRegistry.Load(content);
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
    }
}