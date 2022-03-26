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
        private Dictionary<int, Dictionary<int, MapTileData>> _tilePositions = new();

        public MapData() // fake load map here. later will input json/xml
        {
            // i got this up to  8000 x 8000 tiles! was 27 seconds of wait time though and 5gb ram usage...
            for (int x = 0; x < 100; x++)
            {
                _tilePositions.Add(x, new Dictionary<int, MapTileData>());

                for (int y = 0; y < 100; y++)
                {
                    Random random = new Random();

                    switch (random.Next(0,8))
                    {
                        default:
                            _tilePositions[x].Add(y, new MapTileData("grass"));
                            break;
                        case 2:
                            _tilePositions[x].Add(y, new MapTileData("forest"));
                            break;
                        case 3:
                            _tilePositions[x].Add(y, new MapTileData("path"));
                            break;
                        case 4:
                            _tilePositions[x].Add(y, new MapTileData("boulder"));
                            break;
                        case 1:
                            _tilePositions[x].Add(y, new MapTileData("castle"));
                            break;
                    }
                }
            }
        }

        public MapTileData GetTile(Vector2 pos)
        {
            if (_tilePositions.ContainsKey((int)pos.X) && _tilePositions[(int)pos.X].ContainsKey((int)pos.Y))
                return _tilePositions[(int)pos.X][(int)pos.Y];
            else
                return null;
        }
    }
}
