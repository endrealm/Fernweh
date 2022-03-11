using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;

namespace Core.Scenes.Ingame;

public class GameView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    MapData mapData = new MapData();
    WorldDataRegistry worldDataRegistry = new WorldDataRegistry();
    Player player = new Player();

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
                        worldDataRegistry.GetTile(tileName).frames[0],
                        new Rectangle((int)x * 32 + context.ChatWidth, y * 32, 32, 32),
                        Color.White);
            }
        }

        player.Render(spriteBatch, context);
    }

    public void Load(ContentManager content)
    {
        worldDataRegistry.Load(content);
        player.Load(content);
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        player.Update(deltaTime, context);

        if (KeyboardSnapshot.GetState().GetPressedKeys().Length > 0)
        {
            player.MovePlayer(new Vector2(1, 0));
        }
    }
}