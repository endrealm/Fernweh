using Core.Utils;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Content;

namespace Core.Scenes.Ingame.World
{
    public class MapDataRegistry: ILoadable
    {
        private Dictionary<string, MapData> _mapList = new Dictionary<string, MapData>();
        private string _loadedMap;

        public void Load(ContentLoader content) // fake load tiles here. later will input json
        {
            _mapList.Add("test1", new MapData());
            //_mapList.Add("test2", new MapData());
            _loadedMap = "test1";
        }

        public MapData GetMap(string name)
        {
            if (name == null || !_mapList.ContainsKey(name))
                return null;
            else
                return _mapList[name];
        }

        public MapData GetLoadedMap()
        {
            return _mapList[_loadedMap];
        }

        public MapData LoadMap(string name)
        {
            return _mapList[_loadedMap = name];
        }
    }
}
