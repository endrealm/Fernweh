using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.Content;
using Core.States;
using Core.Scenes.Ingame.Views;
using static Core.Scenes.Ingame.World.MapData;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.IO;

namespace Core.Scenes.Ingame.World
{
    public class Player: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
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
        private bool _movePitchToggle = true;
        private float _movePitchVariance = 0.3f;
        private Texture2D _sprite;
        private MapTileData _previousTileData;
        private MapTileData _targetTileData;

        private WorldGameView _worldRenderer;
        private readonly Vector2[] _discoverTileRadius = {
             new (0, -1), 
             new (-1,0), new (0,0), new (1,0),
             new (0,1),  
        };

        private ISoundPlayer _soundPlayer;

        public Player(IGlobalEventHandler globalEventHandler, IStateManager gameManager, WorldGameView worldRenderer, ISoundPlayer soundPlayer)
        {
            _globalEventHandler = globalEventHandler;
            _gameManager = gameManager;
            _worldRenderer = worldRenderer;
            _soundPlayer = soundPlayer;
        }

        public void Load(ContentLoader content)
        {
            _sprite = content.Load<Texture2D>("Sprites/player.png");
        }

        public void TeleportPlayer(Vector2 mapPos) // can be used to move to spawn
        {
            if (_worldRenderer.mapDataRegistry.GetLoadedMap() == null) return; // cant move if no map loaded
            if (CurrentPos != _targetPos) return; // cant move if currently moving
            //if (_worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32) == null ||
            //    _worldRenderer.tileDataRegistry.GetTile(_worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32).name) == null) return; // cant move to what doesnt exist

            CurrentPos = mapPos * 32;
            _targetPos = CurrentPos;

            DiscoverTiles();
        }

        public void MovePlayer(Vector2 direction)
        {
            if(!_gameManager.ActiveState.AllowMove) return; // cant move if... cant move
            if (_worldRenderer.mapDataRegistry.GetLoadedMap() == null) return; // cant move if no map loaded
            if (CurrentPos != _targetPos) return; // cant move if currently moving

            _previousTileData = _worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32);
            _targetTileData = _worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32 + direction);

            if (_targetTileData == null) return; // cant move to what doesnt exist

            TileData currentTile = _worldRenderer.tileDataRegistry.GetTile(_previousTileData.name);
            TileData targetTile = _worldRenderer.tileDataRegistry.GetTile(_targetTileData.name);

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
            spriteBatch.Draw(
                _sprite,
                new Rectangle(context.ChatWidth + (int)CurrentPos.X - (int)Math.Round((float)(_sprite.Width - 32) / 2), 
                    (int)CurrentPos.Y - (int)Math.Round((float)(_sprite.Height - 32) / 2), 
                    _sprite.Width, _sprite.Height),
                context.WorldTint);
        }

        public void Update(float deltaTime, IngameUpdateContext context)
        {
            if (_firstFrame) // move camera straight to player on first frame
            {
                context.TopLevelUpdateContext.Camera.Position = new Vector2(_targetPos.X - 96, _targetPos.Y - 96);
                _firstFrame = false;
            }
            // move camera to follow player
            else if (CurrentPos == _targetPos)
            {
                var camTargetPos = new Vector2(_targetPos.X - 96, _targetPos.Y - 96); // center player
                var smoothPos = Vector2.SmoothStep(context.TopLevelUpdateContext.Camera.Position, camTargetPos, _camMoveSpeed); // get smoothed position for animation
                context.TopLevelUpdateContext.Camera.Position = new Vector2((int)Math.Ceiling(smoothPos.X), (int)Math.Ceiling(smoothPos.Y)); // convert to int so we dont ruin the pixel sizes
            }

            if (CurrentPos == _targetPos) return;

            if (_moveTimer < _moveTime)
                _moveTimer += deltaTime;
            else
            {
                CurrentPos = CurrentPos + _moveDir * _stepAmount;
                _moveTimer = 0;

                _soundPlayer.PlaySFX("step", _movePitchToggle ? _movePitchVariance : 0);
                _movePitchToggle = !_movePitchToggle;

                if (CurrentPos == _targetPos) // just finished moving
                {
                    DiscoverTiles();

                    if (_previousTileData.firstEnterState != _targetTileData.firstEnterState) // load proper states
                        _gameManager.weakNextID = _targetTileData.firstEnterState;
                    else
                        _gameManager.weakNextID = _targetTileData.enterState;

                    if (_previousTileData.lastLeaveState != _targetTileData.lastLeaveState)
                        _gameManager.LoadState(_previousTileData.lastLeaveState);
                    else
                        _gameManager.LoadState(_previousTileData.leaveState);
                }
            }
        }

        private void DiscoverTiles()
        {
            if (!_worldRenderer.mapDataRegistry.GetLoadedMap().explorable) return; // dont save discovered tiles if map isnt explorable

            foreach (Vector2 offset in _discoverTileRadius)
            {
                if (!_worldRenderer.DiscoveredTiles[_worldRenderer.mapDataRegistry.GetLoadedMap().name].Contains(offset + CurrentPos / 32))
                    _worldRenderer.DiscoveredTiles[_worldRenderer.mapDataRegistry.GetLoadedMap().name].Add(offset + CurrentPos / 32);
            }
        }
    }
}