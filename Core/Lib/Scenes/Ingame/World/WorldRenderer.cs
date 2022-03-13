using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Core.Scenes.Ingame.World
{
    internal class WorldRenderer : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
    {
        public List<Vector2> discoveredTiles = new List<Vector2>();

        MapData mapData = new MapData();
        WorldDataRegistry worldDataRegistry = new WorldDataRegistry();
        Player player = new Player();

        private Vector2 cameraCulling;

        public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
        {
            // get corner of camera screen, we'll render from there on so we dont have to do any loop containing all world tiles
            cameraCulling = new Vector2((int)Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1, (int)Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);

            for (int x = (int)cameraCulling.X; x < (int)cameraCulling.X + 9; x++)
            {
                for (int y = (int)cameraCulling.Y; y < (int)cameraCulling.Y + 9; y++)
                {
                    if (discoveredTiles.Contains(new Vector2(x, y))) // dont render a tile we havent discovered
                    {
                        var tileName = mapData.GetTile(new Vector2(x, y));

                        if (tileName != null) // dont render whats not there :P
                            spriteBatch.Draw(
                                worldDataRegistry.GetTile(tileName).frames[0], // grab sprite
                                new Rectangle((int)x * 32 + context.ChatWidth, y * 32, 32, 32), // get world position to render at
                                context.WorldTint);   
                    }
                }
            }

            player.Render(spriteBatch, context);
        }

        public void Load(ContentManager content)
        {
            worldDataRegistry.Load(content);
            player.Load(content, this);
            player.TeleportPlayer(new Vector2(100,100), mapData, worldDataRegistry);
        }

        public void Update(float deltaTime, IngameUpdateContext context)
        {
            player.Update(deltaTime, context);

            if (Controls.MoveUp())
                player.MovePlayer(new Vector2(0, -1), mapData, worldDataRegistry);
            if (Controls.MoveDown())
                player.MovePlayer(new Vector2(0, 1), mapData, worldDataRegistry);
            if (Controls.MoveLeft())
                player.MovePlayer(new Vector2(-1, 0), mapData, worldDataRegistry);
            if (Controls.MoveRight())
                player.MovePlayer(new Vector2(1, 0), mapData, worldDataRegistry);

            // world tints i liked in case we use them
            //new Color(110, 145, 155)); // night tint
            //new Color(255, 215, 175)); // morning tint
        }
    }
}
