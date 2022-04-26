using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Content;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Core.Scenes.Ingame.World
{
    public class TileDataRegistry: ILoadable
    {
        private Dictionary<string, TileData> _tileList = new Dictionary<string, TileData>();

        public void Load(ContentLoader content) // load all the tiles right now and load their sprites
        {
            List<IArchiveLoader> mods = content.GetMods();
            foreach (IArchiveLoader mod in mods)
            {
                string[] files = mod.LoadAllFiles("*.tile");
                foreach (var file in files)
                {
                    TileData data = JsonConvert.DeserializeObject<TileData>(File.ReadAllText(file));
                    data.LoadSprites(content);
                    _tileList.Add(data.name, data);
                }
            }
            //Console.WriteLine(JsonConvert.SerializeObject(new TileData("castle", new string[] { "Sprites/castle.png" }, TileData.OpenDirection.Down | TileData.OpenDirection.Left | TileData.OpenDirection.Right)));
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
