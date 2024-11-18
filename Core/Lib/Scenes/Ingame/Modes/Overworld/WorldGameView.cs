using System;
using System.Collections.Generic;
using Core.Content;
using Core.Input;
using Core.Scenes.Ingame.Views;
using Core.Scenes.Ingame.World;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.Modes.Overworld;

public class WorldGameView : IGameView
{
    private readonly ContentRegistry _contentRegistry;

    private Vector2 _cameraCulling;
    public Dictionary<string, List<Vector2>> DiscoveredTiles = new();

    public MapDataRegistry MapDataRegistry;
    public readonly Player Player;
    public TileDataRegistry TileDataRegistry;

    public WorldGameView(IGlobalEventHandler eventHandler, IStateManager gameManager, ISoundPlayer soundPlayer,
        ContentRegistry content)
    {
        _contentRegistry = content;
        Player = new Player(eventHandler, gameManager, this, soundPlayer, content);
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        // if theres no map loaded, dont render anything
        if (MapDataRegistry.GetLoadedMap() == null) return;

        // get corner of camera screen, we'll render from there on so we dont have to do any loop containing all world tiles
        _cameraCulling = new Vector2((int) Math.Round(context.TopLevelContext.Camera.Position.X / 32) - 1,
            (int) Math.Round(context.TopLevelContext.Camera.Position.Y / 32) - 1);

        for (var y = (int) _cameraCulling.Y; y < (int) _cameraCulling.Y + 9; y++)
        {
            for (var x = (int) _cameraCulling.X; x < (int) _cameraCulling.X + 9; x++)
            {
                // make sure we have discovered tiles 
                if (!DiscoveredTiles.ContainsKey(MapDataRegistry.GetLoadedMap().name))
                    DiscoveredTiles.Add(MapDataRegistry.GetLoadedMap().name, new List<Vector2>());

                if (DiscoveredTiles[MapDataRegistry.GetLoadedMap().name].Contains(new Vector2(x, y)) ||
                    !MapDataRegistry.GetLoadedMap()
                        .explorable) // only render tiles explored, unless the map isnt set to be explorable
                {
                    var tileData = MapDataRegistry.GetLoadedMap().GetTile(new Vector2(x, y));

                    if (tileData != null) // dont render whats not there :P
                    {
                        var sprite = TileDataRegistry.GetTile(tileData.name).GetSprite(_contentRegistry); // grab sprite

                        spriteBatch.Draw(
                            sprite,
                            new Rectangle(
                                context.ChatWidth + x * 32 - (int) Math.Round((float) (sprite.Width - 32) / 2),
                                y * 32 - (sprite.Height - 32), sprite.Width,
                                sprite.Height), // get world position to render at
                            context.WorldTint);
                    }
                }
            }

            var roundedPos = (float) Math.Ceiling(Player.CurrentPos.Y / 32);
            if (roundedPos == y) // render player right after its current tile
                Player.Render(spriteBatch, context);
        }
    }

    public void Load(ContentLoader content)
    {
        TileDataRegistry = _contentRegistry.tileDataRegistry;
        MapDataRegistry = _contentRegistry.mapDataRegistry.SetupDiscovery(DiscoveredTiles);
    }

    public void Update(float deltaTime, IngameUpdateContext context)
    {
        Player.Update(deltaTime, context);

        if (Controls.MoveUp())
            Player.MovePlayer(new Vector2(0, -1));
        if (Controls.MoveDown())
            Player.MovePlayer(new Vector2(0, 1));
        if (Controls.MoveLeft())
            Player.MovePlayer(new Vector2(-1, 0));
        if (Controls.MoveRight())
            Player.MovePlayer(new Vector2(1, 0));

        // world tints i liked in case we use them
        //new Color(110, 145, 155)); // night tint
        //new Color(255, 215, 175)); // morning tint
    }

    public bool WorldSpacedCoordinates => true;
}