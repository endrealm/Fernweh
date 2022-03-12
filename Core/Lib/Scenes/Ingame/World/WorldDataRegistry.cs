using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Core.Scenes.Ingame.World
{
    internal class WorldDataRegistry: ILoadable
    {
        private Dictionary<string, TileData> tileList = new Dictionary<string, TileData>();

        public void Load(ContentManager content) // fake load tiles here. later will input json/xml
        {
            tileList.Add("grass", new TileData("grass", new Texture2D[] { content.Load<Texture2D>("Sprites/grass") }));
            tileList.Add("forest", new TileData("forest", new Texture2D[] { content.Load<Texture2D>("Sprites/forest") }));
            tileList.Add("path", new TileData("path", new Texture2D[] { content.Load<Texture2D>("Sprites/path") }));
            tileList.Add("boulder", new TileData("boulder", new Texture2D[] { content.Load<Texture2D>("Sprites/boulder") }, TileData.OpenDirections.None));
            tileList.Add("castle", new TileData("castle", new Texture2D[] { content.Load<Texture2D>("Sprites/castle") }, TileData.OpenDirections.Down | TileData.OpenDirections.Left | TileData.OpenDirections.Right));
        }

        public TileData GetTile(string name)
        {
            if (name == null || !tileList.ContainsKey(name))
                return null;
            else
                return tileList[name];
        }
    }
}
