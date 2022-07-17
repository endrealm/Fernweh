using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PipelineExtensionLibrary;
using Newtonsoft.Json;
using Core.Content;

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

        public string name;
        //[JsonIgnore]
        //public Texture2D[] frames;
        public string[] framePaths;
        public int framesPerSecond = 1;

        //public float EncounterChance; // range 0f - 1f
        public OpenDirection openDirections;

        //public void LoadSprites(ContentRegistry contentRegistry)
        //{
        //    frames = new Texture2D[framePaths.Length];
        //    for (int i = 0; i < framePaths.Length; i++)
        //        frames[i] = contentRegistry.pngs[framePaths[i]];
        //}

        public TileData(string name, string[] frames, OpenDirection openDirections = OpenDirection.Up | OpenDirection.Down | OpenDirection.Left | OpenDirection.Right)
        { 
            this.name = name;
            framePaths = frames;
            this.openDirections = openDirections;
        }

        public Texture2D GetSprite(ContentRegistry contentRegistry)
        {
            return contentRegistry.pngs[framePaths[0]]; // return first frame for now, im not sure how ill implement anims yet. but itll probably be calculated in here based from some time parameter
        }

        public bool AllowsDirection(Vector2 direction)
        {
            if (openDirections.HasFlag(OpenDirection.Up) && direction.Y == -1)
                return true;

            if (openDirections.HasFlag(OpenDirection.Down) && direction.Y == 1)
                return true;

            if (openDirections.HasFlag(OpenDirection.Left) && direction.X == -1)
                return true;

            if (openDirections.HasFlag(OpenDirection.Right) && direction.X == 1)
                return true;

            return false;
        }
    }
}
