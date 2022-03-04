using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.Scenes.Ingame.World
{
    internal class MapData
    {
        private Dictionary<Vector2, string> tilePositions = new Dictionary<Vector2, string>();  // convert to matrix?

        public MapData() // fake load map here. later will input json/xml
        {
            // i got this up to  8000 x 8000 tiles! was 27 seconds of wait time though and 5gb ram usage...
            for (int x = 0; x < 500; x++)
            {
                for (int y = 0; y < 500; y++)
                {
                    Random random = new Random();

                    if(random.Next(0,2) == 1)
                    tilePositions.Add(new Vector2(x,y), "grass");
                    else
                        tilePositions.Add(new Vector2(x, y), "forest");
                }
            }
        }

        public string GetTile(Vector2 pos)
        {
            if (tilePositions.ContainsKey(pos))
                return tilePositions[pos];
            else
                return null;
        }
    }
}
