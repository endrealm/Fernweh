using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.States;

namespace Core.Scenes.Ingame.World
{
    internal class Player: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
    {
        private readonly IGlobalEventHandler _globalEventHandler;
        private readonly GameManager _gameManager;

        private readonly float _moveTime = 0.2f;
        private readonly float _camMoveSpeed = 0.3f;
        private readonly int _stepAmount = 8;

        private float _moveTimer;
        private bool _firstFrame = true;

        private Vector2 CurrentPos;
        private Vector2 _targetPos;
        private Vector2 _moveDir;
        private Texture2D _sprite;
        private string _previousTileName;
        private string _targetTileName;

        private WorldRenderer _worldRenderer;
        private readonly Vector2[] _discoverTileRadius = {
             new (0, -1), 
             new (-1,0), new (0,0), new (1,0),
             new (0,1),  
        };

        public Player(IGlobalEventHandler globalEventHandler, GameManager gameManager, WorldRenderer worldRenderer)
        {
            _globalEventHandler = globalEventHandler;
            _gameManager = gameManager;
            _worldRenderer = worldRenderer;
        }

        public void Load(ContentManager content)
        {
            _sprite = content.Load<Texture2D>("Sprites/player");
        }

        public void TeleportPlayer(Vector2 mapPos) // can be used to move to spawn
        {
            if (CurrentPos != _targetPos) return; // cant move if currently moving
            if (_worldRenderer.worldDataRegistry.GetTile(_worldRenderer.mapData.GetTile(CurrentPos / 32)) == null) return; // cant move to what doesnt exist

            CurrentPos = mapPos * 32;
            _targetPos = CurrentPos;

            DiscoverTiles();
        }

        public void MovePlayer(Vector2 direction)
        {
            if(!_gameManager.ActiveState.AllowMove) return;
            if (CurrentPos != _targetPos) return; // cant move if currently moving

            _previousTileName = _worldRenderer.mapData.GetTile(CurrentPos / 32);
            _targetTileName = _worldRenderer.mapData.GetTile(CurrentPos / 32 + direction);

            TileData currentTile = _worldRenderer.worldDataRegistry.GetTile(_previousTileName);
            TileData targetTile = _worldRenderer.worldDataRegistry.GetTile(_targetTileName);

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

                if (CurrentPos == _targetPos) // just finished moving
                {
                    DiscoverTiles();
                    //_gameManager.LoadState("leave_" + _previousTileName);
                    //_gameManager.weakNextId = "enter_" + _targetTileName;
                }
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