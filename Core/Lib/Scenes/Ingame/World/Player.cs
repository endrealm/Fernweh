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
    internal class Player: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable<WorldRenderer>
    {
        public Vector2 currentPos = new Vector2(0,0);
        private float moveTime = 0.2f;
        private float camMoveSpeed = 0.3f;
        private int stepAmount = 8;

        private float moveTimer;
        private bool firstFrame = true;

        private Vector2 targetPos;
        private Vector2 moveDir;
        private Texture2D sprite;

        private WorldRenderer _worldRenderer;

        private Vector2[] discoverTileRadius = new Vector2[] 
        {
             new Vector2(0, -1), 
             new Vector2(-1,0), new Vector2(0,0), new Vector2(1,0),
             new Vector2(0,1),  
        };

        public void Load(ContentManager content, WorldRenderer worldRenderer)
        {
            sprite = content.Load<Texture2D>("Sprites/player");
            _worldRenderer = worldRenderer;
        }

        public void TeleportPlayer(Vector2 mapPos, MapData mapData, WorldDataRegistry worldDataRegistry) // can be used to move to spawn
        {
            if (currentPos != targetPos) return; // cant move if currently moving
            if (worldDataRegistry.GetTile(mapData.GetTile(currentPos / 32)) == null) return; // cant move to what doesnt exist

            currentPos = mapPos * 32;
            targetPos = currentPos;

            DiscoverTiles();
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
                targetPos = currentPos + direction * 32;
                moveTimer = moveTime;
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

            if (moveTimer < moveTime)
                moveTimer += deltaTime;
            else
            {
                currentPos = currentPos + moveDir * stepAmount;
                moveTimer = 0;

                if (currentPos == targetPos) DiscoverTiles();
            }
        }

        private void DiscoverTiles()
        {
            foreach (Vector2 offset in discoverTileRadius)
            {
                if (!_worldRenderer.discoveredTiles.Contains(offset + currentPos / 32))
                    _worldRenderer.discoveredTiles.Add(offset + currentPos / 32);
            }
        }
    }
}