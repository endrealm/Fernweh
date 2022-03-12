using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.Scenes.Ingame;
using System.Collections.Generic;
using System.Text;

namespace Core.Scenes.Ingame.World
{
    internal class Player: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
    {
        public Vector2 currentPos = new Vector2(0,0);
        private float moveSpeed = 0.6f;
        private float camMoveSpeed = 0.3f;
        private int stepAmount = 8;

        private Vector2 targetPos;
        private Vector2 tempPos;
        private Vector2 moveDir;

        private bool firstFrame = true;
        private Texture2D sprite;

        public void Load(ContentManager content)
        {
            sprite = content.Load<Texture2D>("Sprites/player");
        }

        public void MovePlayer(Vector2 direction, MapData mapData, WorldDataRegistry worldDataRegistry)
        {
            if (currentPos != targetPos) return; // cant move if currently moving

            TileData currentTile = worldDataRegistry.GetTile(mapData.GetTile(currentPos / 32));
            TileData targetTile = worldDataRegistry.GetTile(mapData.GetTile(currentPos / 32 + direction));

            if (targetTile == null) return; // cant move to what doesnt exist

            if (currentTile.AllowsDirection(direction) && // check both tiles allow the direction
                targetTile.AllowsDirection(direction * new Vector2(-1, -1)))
            {
                moveDir = direction;
                tempPos = currentPos;
                targetPos = currentPos + direction * 32;
            }
        }

        public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
        {
            if (firstFrame) // move camera straight to player on first frame
            {
                context.TopLevelContext.Camera.Position = new Vector2(targetPos.X - 96, targetPos.Y - 96);
                firstFrame = false;
            }
            // move camera to follow player
            else if(currentPos == targetPos)
            {
                var camTargetPos = new Vector2(targetPos.X - 96, targetPos.Y - 96); // center player
                var smoothPos = Vector2.SmoothStep(context.TopLevelContext.Camera.Position, camTargetPos, camMoveSpeed); // get smoothed position for animation
                context.TopLevelContext.Camera.Position = new Vector2((int)Math.Ceiling(smoothPos.X), (int)Math.Ceiling(smoothPos.Y)); // convert to int so we dont ruin the pixel sizes
            }

            // draw player
            spriteBatch.Draw(
                sprite,
                new Rectangle((int)Math.Round(currentPos.X) + context.ChatWidth, (int)currentPos.Y, 32, 32),
                context.WorldTint);
        }

        public void Update(float deltaTime, IngameUpdateContext context)
        {
            if (currentPos == targetPos) return;

            if (currentPos == tempPos)
                tempPos = currentPos + moveDir * stepAmount;
            else
                currentPos = Vector2.SmoothStep(currentPos, tempPos, moveSpeed);
        }
    }
}