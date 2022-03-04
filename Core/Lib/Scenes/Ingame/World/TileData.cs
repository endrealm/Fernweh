using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.World
{
    internal class TileData
    {
        public string _name;
        public Texture2D _sprite;

        public TileData(string name, Texture2D sprite)
        { 
            _name = name;
            _sprite = sprite;
        }
    }
}
