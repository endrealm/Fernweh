using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.World
{
    internal class WorldDataRegistry: ILoadable
    {
        private Dictionary<string, TileData> _tileList = new Dictionary<string, TileData>();

        public void Load(ContentLoader content) // fake load tiles here. later will input json/xml
        {
            _tileList.Add("grass", new TileData("grass", new Texture2D[] { content.LoadTexture("Sprites/grass.png") }));
            _tileList.Add("forest", new TileData("forest", new Texture2D[] { content.LoadTexture("Sprites/forest.png") }));
            _tileList.Add("path", new TileData("path", new Texture2D[] { content.LoadTexture("Sprites/path.png") }));
            _tileList.Add("boulder", new TileData("boulder", new Texture2D[] { content.LoadTexture("Sprites/boulder.png") }, TileData.OpenDirection.None));
            _tileList.Add("castle", new TileData("castle", new Texture2D[] { content.LoadTexture("Sprites/castle.png") }, TileData.OpenDirection.Down | TileData.OpenDirection.Left | TileData.OpenDirection.Right));
        }

        public TileData GetTile(string name)
        {
            if (name == null || !_tileList.ContainsKey(name))
                return null;
            else
                return _tileList[name];
        }
    }
}
