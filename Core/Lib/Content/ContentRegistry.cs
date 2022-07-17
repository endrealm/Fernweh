using Core.Content;
using Core.Scenes.Ingame;
using Core.Scenes.Ingame.Modes.Battle.Impl;
using Core.Scenes.Ingame.World;
using Core.Scenes.Modding;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Content
{
    public class ContentRegistry
    {
        public SoundRegistry soundRegistry;
        public TileDataRegistry tileDataRegistry;
        public MapDataRegistry mapDataRegistry;
        public DynamicBattleSpriteManager dynamicBattleSpriteManager;

        public Dictionary<string, TileData> tiles = new();
        public Dictionary<string, MapData> maps = new();
        public Dictionary<string, Texture2D> pngs = new();
        public Dictionary<string, SoundEffect> wavs = new();

        public async void LoadAllContent(ContentLoader content) // load all files and sort them by extensions
        {
                List<Mod> mods = content.ModLoader.ActiveModOrder;
                foreach (Mod mod in mods)
                {
                    string[] files = mod.Archive.LoadAllFiles("*.*");
                    foreach (var file in files)
                    {
                        string name = System.IO.Path.GetFileNameWithoutExtension(file);
                        switch (System.IO.Path.GetExtension(file))
                        {
                            case ".map":
                                if (!maps.ContainsKey(name))
                                    maps.Add(name, JsonConvert.DeserializeObject<MapData>(mod.Archive.LoadFile(file)));
                                break;

                            case ".tile":
                                if (!tiles.ContainsKey(name))
                                    tiles.Add(name, JsonConvert.DeserializeObject<TileData>(mod.Archive.LoadFile(file)));
                                break;

                            case ".png":
                                if(!pngs.ContainsKey(name))
                                    pngs.Add(name, content.Load<Texture2D>(file, mod.Id));
                                break;

                            case ".wav":
                                if (!wavs.ContainsKey(name))
                                    wavs.Add(name, SoundEffect.FromStream(mod.Archive.LoadFileAsStream(file)));
                                break;

                            default:
                                break;
                        }
                    }
                }
            tileDataRegistry = new TileDataRegistry(this);
            mapDataRegistry = new MapDataRegistry(this);
            dynamicBattleSpriteManager = new DynamicBattleSpriteManager(this);
            soundRegistry = new SoundRegistry(this);
        }
    }
}
