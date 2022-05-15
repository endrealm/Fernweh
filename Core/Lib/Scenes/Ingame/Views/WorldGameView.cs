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
using Core.Content;
using Core.States;
using Newtonsoft.Json;

namespace Core.Scenes.Ingame.Views;

public class WorldGameView: IGameView, IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    public Dictionary<string, List<Vector2>> DiscoveredTiles = new();

    public MapDataRegistry mapDataRegistry = new MapDataRegistry();
    public TileDataRegistry tileDataRegistry = new TileDataRegistry();
    public Player player;

    private Vector2 _cameraCulling;

    public WorldGameView(IGlobalEventHandler eventHandler, IStateManager gameManager, ISoundPlayer soundPlayer)
    {
        player = new Player(eventHandler, gameManager, this, soundPlayer);
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // get corner of camera screen, we'll render from there on so we dont have to do any loop containing all world tiles
        _cameraCulling = new Vector2((int)Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1, (int)Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);

        for (int x = (int)_cameraCulling.X; x < (int)_cameraCulling.X + 9; x++)
        {
            for (int y = (int)_cameraCulling.Y; y < (int)_cameraCulling.Y + 9; y++)
            {
                if (DiscoveredTiles[mapDataRegistry.GetLoadedMap().name].Contains(new Vector2(x, y)) || !mapDataRegistry.GetLoadedMap().explorable) // only render tiles explored, unless the map isnt set to be explorable
                {
                    var tileData = mapDataRegistry.GetLoadedMap().GetTile(new Vector2(x, y));

                    if (tileData != null) // dont render whats not there :P
                        spriteBatch.Draw(
                            tileDataRegistry.GetTile(tileData.name).GetSprite(), // grab sprite
                            new Rectangle((int)x * 32 + context.ChatWidth, y * 32, 32, 32), // get world position to render at
                            context.WorldTint);
                }
            }
        }

        player.Render(spriteBatch, context);
    }

    public void Load(ContentLoader content)
    {
        tileDataRegistry.Load(content);
        mapDataRegistry.Load(content, DiscoveredTiles);

        player.Load(content);
        player.TeleportPlayer(new Vector2(7, 10));
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

        // world tints i liked in case we use them
        //new Color(110, 145, 155)); // night tint
        //new Color(255, 215, 175)); // morning tint
    }

    public bool WorldSpacedCoordinates => true;
}