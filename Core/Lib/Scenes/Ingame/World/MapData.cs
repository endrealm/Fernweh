using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace Core.Scenes.Ingame.World
{
    public class MapData
    {
        public string name;
        public bool explorable = true;
        [JsonProperty("tilePositions")]
        private Dictionary<Vector2, MapTileData> _tilePositions = new Dictionary<Vector2, MapTileData>();  // convert to matrix?

        public class MapTileData
        {
            public MapTileData(string _name, string _enterState = null, string _leaveState = null)
            {
                name = _name;
                enterState = (_enterState == null) ? "enter_" + _name : _enterState;
                leaveState = (_leaveState == null) ? "leave_" + _name : _leaveState;
            }

            public string name;
            public string enterState;
            public string leaveState;
        }

        public MapData() // fake load map here. later will input json/xml
        {
            // i got this up to  8000 x 8000 tiles! was 27 seconds of wait time though and 5gb ram usage...
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    Random random = new Random();

                    switch (random.Next(0,8))
                    {
                        default:
                            _tilePositions.Add(new Vector2(x, y), new MapTileData("grass"));
                            break;
                        case 2:
                            _tilePositions.Add(new Vector2(x, y), new MapTileData("forest"));
                            break;
                        case 3:
                            _tilePositions.Add(new Vector2(x, y), new MapTileData("path"));
                            break;
                        case 4:
                            _tilePositions.Add(new Vector2(x, y), new MapTileData("boulder"));
                            break;
                        case 1:
                            _tilePositions.Add(new Vector2(x, y), new MapTileData("castle"));
                            break;
                    }
                }
            }
        }

        public MapTileData GetTile(Vector2 pos)
        {
            if (_tilePositions.ContainsKey(pos))
                return _tilePositions[pos];
            else
                return null;
        }
    }
}
