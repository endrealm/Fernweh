using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;
using Microsoft.Xna.Framework.Input;

namespace Core.Scenes.Ingame.World
{
    internal class WorldRenderer : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
    {
        MapData mapData = new MapData();
        WorldDataRegistry worldDataRegistry = new WorldDataRegistry();
        Player player = new Player();

        private Vector2 cameraCulling;

        public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
        {
            cameraCulling = new Vector2((int)Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1, (int)Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);

            for (int x = (int)cameraCulling.X; x < (int)cameraCulling.X + 9; x++)
            {
                for (int y = (int)cameraCulling.Y; y < (int)cameraCulling.Y + 9; y++)
                {
                    var tileName = mapData.GetTile(new Vector2(x, y));

                    if (tileName != null)
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

            if (Controls.MoveUp())
                player.MovePlayer(new Vector2(0, -1));
            if (Controls.MoveDown())
                player.MovePlayer(new Vector2(0, 1));
            if (Controls.MoveLeft())
                player.MovePlayer(new Vector2(-1, 0));
            if (Controls.MoveRight())
                player.MovePlayer(new Vector2(1, 0));
        }
    }
}
