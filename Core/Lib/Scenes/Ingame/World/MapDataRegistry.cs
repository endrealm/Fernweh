using Core.Utils;
using System.Collections.Generic;
using Core.Content;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace Core.Scenes.Ingame.World
{
    public class MapDataRegistry
    {
        private Dictionary<string, MapData> _mapList = new();
        private string _loadedMap;

        public void Load(ContentLoader content, Dictionary<string, List<Vector2>> discoveredTiles) // load all maps here and load the first one
        {
            List<IArchiveLoader> mods = content.GetMods();
            foreach (IArchiveLoader mod in mods)
            {
                string[] files = mod.LoadAllFiles("*.map");
                for (int i = 0; i < files.Length; i++)
                {
                    MapData data = JsonConvert.DeserializeObject<MapData>(mod.LoadFile(files[i]));
                    _mapList.Add(data.name, data);
                    discoveredTiles.Add(data.name, new List<Vector2>());
                }
            }
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
            if (_loadedMap == null)
                return null;
            else
                return _mapList[_loadedMap];
        }

        public string GetLoadedMapName()
        {
            return _loadedMap;
        }

        public MapData LoadMap(string name)
        {
            if (name == null || !_mapList.ContainsKey(name))
                return null;
            else
                return _mapList[_loadedMap = name];
        }

        private MapData CreateMapData(ContentLoader content, string path)
        {
            return JsonConvert.DeserializeObject<MapData>(content.Load<string>(path));
        }
    }
}
