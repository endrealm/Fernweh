﻿using Core.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Core.Scenes.Ingame.World;
using System;
using Core.Input;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Core.States;

namespace Core.Scenes.Ingame.World
{
    internal class WorldRenderer : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
    {
        public List<Vector2> DiscoveredTiles = new List<Vector2>();

        private MapData _mapData = new MapData();
        private WorldDataRegistry _worldDataRegistry = new WorldDataRegistry();
        private Player _player;

        private Vector2 _cameraCulling;

        public WorldRenderer(IGlobalEventHandler eventHandler, IStateManager gameManager)
        {
            _player = new Player(eventHandler, gameManager);
        }

        public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
        {
            // get corner of camera screen, we'll render from there on so we dont have to do any loop containing all world tiles
            _cameraCulling = new Vector2((int)Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1, (int)Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);

            for (int x = (int)_cameraCulling.X; x < (int)_cameraCulling.X + 9; x++)
            {
                for (int y = (int)_cameraCulling.Y; y < (int)_cameraCulling.Y + 9; y++)
                {
                    if (DiscoveredTiles.Contains(new Vector2(x, y))) // dont render a tile we havent discovered
                    {
                        var tileName = _mapData.GetTile(new Vector2(x, y));

                        if (tileName != null) // dont render whats not there :P
                            spriteBatch.Draw(
                                _worldDataRegistry.GetTile(tileName).Frames[0], // grab sprite
                                new Rectangle((int)x * 32 + context.ChatWidth, y * 32, 32, 32), // get world position to render at
                                context.WorldTint);   
                    }
                }
            }

            _player.Render(spriteBatch, context);
        }

        public void Load(ContentManager content)
        {
            _worldDataRegistry.Load(content);
            _player.Load(content, this);
            _player.TeleportPlayer(new Vector2(100,100), _mapData, _worldDataRegistry);
        }

        public void Update(float deltaTime, IngameUpdateContext context)
        {
            _player.Update(deltaTime, context);

            if (Controls.MoveUp())
                _player.MovePlayer(new Vector2(0, -1), _mapData, _worldDataRegistry);
            if (Controls.MoveDown())
                _player.MovePlayer(new Vector2(0, 1), _mapData, _worldDataRegistry);
            if (Controls.MoveLeft())
                _player.MovePlayer(new Vector2(-1, 0), _mapData, _worldDataRegistry);
            if (Controls.MoveRight())
                _player.MovePlayer(new Vector2(1, 0), _mapData, _worldDataRegistry);

            // world tints i liked in case we use them
            //new Color(110, 145, 155)); // night tint
            //new Color(255, 215, 175)); // morning tint
        }
    }
}
