using Core.Utils;
using System.Collections.Generic;
using Core.Content;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Core.Scenes.Modding;
using System;

namespace Core.Scenes.Ingame.World
{
    public class MapDataRegistry
    {
        //private Dictionary<string, MapData> _mapList = new();
        private ContentRegistry _contentRegistry;
        private string _loadedMap;

        public MapDataRegistry(ContentRegistry content) // load all maps here and load the first one
        {
            _contentRegistry = content;
        }

        public MapDataRegistry SetupDiscovery(Dictionary<string, List<Vector2>> discoveredTiles)
        {
            foreach (var item in _contentRegistry.maps)
                discoveredTiles.Add(item.Key, new List<Vector2>());

            return this;
        }

        public MapData GetMap(string name)
        {
            if (name == null || !_contentRegistry.maps.ContainsKey(name))
                return null;
            else
                return _contentRegistry.maps[name];
        }

        public MapData GetLoadedMap()
        {
            if (_loadedMap == null)
                return null;
            else
                return _contentRegistry.maps[_loadedMap];
        }

        public string GetLoadedMapName()
        {
            return _loadedMap;
        }

        public MapData LoadMap(string name)
        {
            if (name == null || !_contentRegistry.maps.ContainsKey(name))
                return null;
            else
                return _contentRegistry.maps[_loadedMap = name];
        }

        private MapData CreateMapData(ContentLoader content, string path)
        {
            return JsonConvert.DeserializeObject<MapData>(content.Load<string>(path));
        }
    }
}
