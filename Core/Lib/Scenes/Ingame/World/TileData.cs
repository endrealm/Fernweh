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
    public class TileData
    {
        [Flags] public enum OpenDirection
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        public string Name;
        public Texture2D[] Frames;
        public int FramesPerSecond;

        public float EncounterChance; // range 0f - 1f
        public OpenDirection OpenDirections;

        public DialogTranslationData OnEnter;

        public TileData(string name, Texture2D[] frames, OpenDirection openDirections = OpenDirection.Up | OpenDirection.Down | OpenDirection.Left | OpenDirection.Right)
        { 
            Name = name;
            Frames = frames;
            OpenDirections = openDirections;
        }

        public bool AllowsDirection(Vector2 direction)
        {
            if (OpenDirections.HasFlag(OpenDirection.Up) && direction.Y == -1)
                return true;

            if (OpenDirections.HasFlag(OpenDirection.Down) && direction.Y == 1)
                return true;

            if (OpenDirections.HasFlag(OpenDirection.Left) && direction.X == -1)
                return true;

            if (OpenDirections.HasFlag(OpenDirection.Right) && direction.X == 1)
                return true;

            return false;
        }
    }
}
