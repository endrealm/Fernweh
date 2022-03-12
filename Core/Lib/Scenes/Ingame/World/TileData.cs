using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.World
{
    [Serializable]
    internal class TileData
    {
        [Flags] public enum OpenDirections
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        public string name;
        public Texture2D[] frames;
        public int framesPerSecond;

        public float encounterChance; // range 0f - 1f
        public OpenDirections openDirections = OpenDirections.Up | OpenDirections.Down | OpenDirections.Left | OpenDirections.Right;

        public DialogTranslationData onEnter;

        public TileData(string _name, Texture2D[] _frames, OpenDirections _openDirections = OpenDirections.Up | OpenDirections.Down | OpenDirections.Left | OpenDirections.Right)
        { 
            name = _name;
            frames = _frames;
            openDirections = _openDirections;
        }

        public bool AllowsDirection(Vector2 direction)
        {
            if (openDirections.HasFlag(TileData.OpenDirections.Up) && direction.Y == -1)
                return true;

            if (openDirections.HasFlag(TileData.OpenDirections.Down) && direction.Y == 1)
                return true;

            if (openDirections.HasFlag(TileData.OpenDirections.Left) && direction.X == -1)
                return true;

            if (openDirections.HasFlag(TileData.OpenDirections.Right) && direction.X == 1)
                return true;

            return false;
        }
    }
}
