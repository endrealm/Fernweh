using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.World
{
    internal class MapData
    {
        private Dictionary<Vector2, string> _tilePositions = new Dictionary<Vector2, string>();  // convert to matrix?

        public MapData() // fake load map here. later will input json/xml
        {
            // i got this up to  8000 x 8000 tiles! was 27 seconds of wait time though and 5gb ram usage...
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    Random random = new Random();

                    switch (random.Next(0,8))
                    {
                        default:
                            _tilePositions.Add(new Vector2(x, y), "grass");
                            break;
                        case 2:
                            _tilePositions.Add(new Vector2(x, y), "forest");
                            break;
                        case 3:
                            _tilePositions.Add(new Vector2(x, y), "path");
                            break;
                        case 4:
                            _tilePositions.Add(new Vector2(x, y), "boulder");
                            break;
                        case 1:
                            _tilePositions.Add(new Vector2(x, y), "castle");
                            break;
                    }
                }
            }
        }

        public string GetTile(Vector2 pos)
        {
            if (_tilePositions.ContainsKey(pos))
                return _tilePositions[pos];
            else
                return null;
        }
    }
}
