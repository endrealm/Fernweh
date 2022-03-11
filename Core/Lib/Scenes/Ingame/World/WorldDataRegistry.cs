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
        }

        public TileData GetTile(string name)
        {
            if (tileList.ContainsKey(name))
                return tileList[name];
            else
                return null;
        }
    }
}
