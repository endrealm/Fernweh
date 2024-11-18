using Core.Content;

namespace Core.Scenes.Ingame.World;

public class TileDataRegistry
{
    //private Dictionary<string, TileData> _tileList = new Dictionary<string, TileData>();
    private readonly ContentRegistry _content;

    public TileDataRegistry(ContentRegistry content) // load all the tiles right now and load their sprites
    {
        _content = content;

        //Console.WriteLine(JsonConvert.SerializeObject(new TileData("castle", new string[] { "Sprites/castle.png" }, TileData.OpenDirection.Down | TileData.OpenDirection.Left | TileData.OpenDirection.Right)));
    }

    public TileData GetTile(string name)
    {
        if (name == null || !_content.tiles.ContainsKey(name))
            return null;
        return _content.tiles[name];
    }
}