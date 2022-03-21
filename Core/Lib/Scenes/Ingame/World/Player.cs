using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.States;

namespace Core.Scenes.Ingame.World
{
    internal class Player: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable<WorldRenderer>
    {
        private readonly IGlobalEventHandler _globalEventHandler;
        private readonly IStateManager _gameManager;
        public Vector2 CurrentPos = new (0,0);
        private float _moveTime = 0.2f;
        private float _camMoveSpeed = 0.3f;
        private int _stepAmount = 8;

        private float _moveTimer;
        private bool _firstFrame = true;

        private Vector2 _targetPos;
        private Vector2 _moveDir;
        private Texture2D _sprite;

        private WorldRenderer _worldRenderer;
        private readonly Vector2[] _discoverTileRadius = {
             new (0, -1), 
             new (-1,0), new (0,0), new (1,0),
             new (0,1),  
        };

        public Player(IGlobalEventHandler globalEventHandler, IStateManager gameManager)
        {
            _globalEventHandler = globalEventHandler;
            _gameManager = gameManager;
        }

        public void Load(ContentManager content, WorldRenderer worldRenderer)
        {
            _sprite = content.Load<Texture2D>("Sprites/player");
            _worldRenderer = worldRenderer;
        }

        public void TeleportPlayer(Vector2 mapPos, MapData mapData, WorldDataRegistry worldDataRegistry) // can be used to move to spawn
        {
            if (CurrentPos != _targetPos) return; // cant move if currently moving
            if (worldDataRegistry.GetTile(mapData.GetTile(CurrentPos / 32)) == null) return; // cant move to what doesnt exist

            CurrentPos = mapPos * 32;
            _targetPos = CurrentPos;

            DiscoverTiles();
        }

        public void MovePlayer(Vector2 direction, MapData mapData, WorldDataRegistry worldDataRegistry)
        {
            if(!_gameManager.ActiveState.AllowMove) return;
            if (CurrentPos != _targetPos) return; // cant move if currently moving

            TileData currentTile = worldDataRegistry.GetTile(mapData.GetTile(CurrentPos / 32));
            TileData targetTile = worldDataRegistry.GetTile(mapData.GetTile(CurrentPos / 32 + direction));

            if (targetTile == null) return; // cant move to what doesnt exist

            if (currentTile.AllowsDirection(direction) && // check both tiles allow the direction
                targetTile.AllowsDirection(direction * new Vector2(-1, -1)))
            {
                _globalEventHandler.EmitPrePlayerMoveEvent();
                _moveDir = direction;
                _targetPos = CurrentPos + direction * 32;
                _moveTimer = _moveTime;
            }
        }

        public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
        {
            if (_firstFrame) // move camera straight to player on first frame
            {
                context.TopLevelContext.Camera.Position = new Vector2(_targetPos.X - 96, _targetPos.Y - 96);
                _firstFrame = false;
            }
            // move camera to follow player
            else if(CurrentPos == _targetPos)
            {
                var camTargetPos = new Vector2(_targetPos.X - 96, _targetPos.Y - 96); // center player
                var smoothPos = Vector2.SmoothStep(context.TopLevelContext.Camera.Position, camTargetPos, _camMoveSpeed); // get smoothed position for animation
                context.TopLevelContext.Camera.Position = new Vector2((int)Math.Ceiling(smoothPos.X), (int)Math.Ceiling(smoothPos.Y)); // convert to int so we dont ruin the pixel sizes
            }

            // draw player
            spriteBatch.Draw(
                _sprite,
                new Rectangle((int)Math.Round(CurrentPos.X) + context.ChatWidth, (int)CurrentPos.Y, 32, 32),
                context.WorldTint);
        }

        public void Update(float deltaTime, IngameUpdateContext context)
        {
            if (CurrentPos == _targetPos) return;

            if (_moveTimer < _moveTime)
                _moveTimer += deltaTime;
            else
            {
                CurrentPos = CurrentPos + _moveDir * _stepAmount;
                _moveTimer = 0;

                if (CurrentPos == _targetPos) DiscoverTiles();
            }
        }

        private void DiscoverTiles()
        {
            foreach (Vector2 offset in _discoverTileRadius)
            {
                if (!_worldRenderer.DiscoveredTiles.Contains(offset + CurrentPos / 32))
                    _worldRenderer.DiscoveredTiles.Add(offset + CurrentPos / 32);
            }
        }
    }
}