using System;
using Core.Content;
using Core.Scenes.Ingame.Modes.Overworld;
using Core.Scenes.Ingame.Views;
using Core.States;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.World;

public class Player : IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>
{
    private readonly float _camMoveSpeed = 0.3f;

    private readonly Vector2[] _discoverTileRadius =
    {
        new(0, -1),
        new(-1, 0), new(0, 0), new(1, 0),
        new(0, 1)
    };

    private readonly IStateManager _gameManager;
    private readonly IGlobalEventHandler _globalEventHandler;
    private readonly float _movePitchVariance = 0.3f;
    private readonly float _moveTime = 0.2f;

    private readonly ISoundPlayer _soundPlayer;
    private readonly Texture2D _sprite;
    private readonly int _stepAmount = 8;

    private readonly WorldGameView _worldRenderer;
    private bool _firstFrame = true;
    private Vector2 _moveDir;
    private bool _movePitchToggle = true;

    private float _moveTimer;
    private MapTileData _previousTileData;

    private Vector2 _targetPos;
    private MapTileData _targetTileData;
    public Vector2 CurrentPos = new(0, 0);

    public Player(IGlobalEventHandler globalEventHandler, IStateManager gameManager, WorldGameView worldRenderer,
        ISoundPlayer soundPlayer, ContentRegistry content)
    {
        _globalEventHandler = globalEventHandler;
        _gameManager = gameManager;
        _worldRenderer = worldRenderer;
        _soundPlayer = soundPlayer;
        _sprite = content.pngs["player"];
    }

    public void Render(SpriteBatch spriteBatch, IngameRenderContext context)
    {
        spriteBatch.Draw(
            _sprite,
            new Rectangle(context.ChatWidth + (int) CurrentPos.X - (int) Math.Round((float) (_sprite.Width - 32) / 2),
                (int) CurrentPos.Y - (int) Math.Round((float) (_sprite.Height - 32) / 2),
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
            var smoothPos =
                Vector2.SmoothStep(context.TopLevelUpdateContext.Camera.Position, camTargetPos,
                    _camMoveSpeed); // get smoothed position for animation
            context.TopLevelUpdateContext.Camera.Position =
                new Vector2((int) Math.Ceiling(smoothPos.X),
                    (int) Math.Ceiling(smoothPos.Y)); // convert to int so we dont ruin the pixel sizes
        }

        if (CurrentPos == _targetPos) return;

        if (_moveTimer < _moveTime)
        {
            _moveTimer += deltaTime;
        }
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

    public void TeleportPlayer(Vector2 mapPos) // can be used to move to spawn
    {
        if (_worldRenderer.MapDataRegistry.GetLoadedMap() == null) return; // cant move if no map loaded
        if (CurrentPos != _targetPos) return; // cant move if currently moving
        //if (_worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32) == null ||
        //    _worldRenderer.tileDataRegistry.GetTile(_worldRenderer.mapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32).name) == null) return; // cant move to what doesnt exist

        CurrentPos = mapPos * 32;
        _targetPos = CurrentPos;

        DiscoverTiles();
    }

    public void MovePlayer(Vector2 direction)
    {
        if (!_gameManager.ActiveState.AllowMove) return; // cant move if... cant move
        if (_worldRenderer.MapDataRegistry.GetLoadedMap() == null) return; // cant move if no map loaded
        if (CurrentPos != _targetPos) return; // cant move if currently moving

        _previousTileData = _worldRenderer.MapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32);
        _targetTileData = _worldRenderer.MapDataRegistry.GetLoadedMap().GetTile(CurrentPos / 32 + direction);

        if (_targetTileData == null) return; // cant move to what doesnt exist

        var currentTile = _worldRenderer.TileDataRegistry.GetTile(_previousTileData.name);
        var targetTile = _worldRenderer.TileDataRegistry.GetTile(_targetTileData.name);

        if (currentTile.AllowsDirection(direction) && // check both tiles allow the direction
            targetTile.AllowsDirection(direction * new Vector2(-1, -1)))
        {
            _globalEventHandler.EmitPrePlayerMoveEvent();
            _moveDir = direction;
            _targetPos = CurrentPos + direction * 32;
            _moveTimer = _moveTime;
        }
    }

    private void DiscoverTiles()
    {
        if (!_worldRenderer.MapDataRegistry.GetLoadedMap().explorable)
            return; // dont save discovered tiles if map isnt explorable

        foreach (var offset in _discoverTileRadius)
            if (!_worldRenderer.DiscoveredTiles[_worldRenderer.MapDataRegistry.GetLoadedMap().name]
                    .Contains(offset + CurrentPos / 32))
                _worldRenderer.DiscoveredTiles[_worldRenderer.MapDataRegistry.GetLoadedMap().name]
                    .Add(offset + CurrentPos / 32);
    }
}