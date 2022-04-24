using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Core;
using Core.Content;

namespace CrossPlatformDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var modDir = args.Length > 0 ? args[0] : null;

            var fileLoader = new FileLoader();
            
            var mods = new List<IArchiveLoader>();
            var scanDirs = ScanForMods(fileLoader);
            scanDirs.ForEach(dir =>
            {
                mods.Add(dir);
            });
            if (modDir != null)
            {
                mods.Add(fileLoader.LoadArchive(modDir));
            }
            
            using (var game = new CoreGame(new MouseClickInput(),  mods))
                game.Run();
        }

        private static List<IArchiveLoader> ScanForMods(FileLoader loader)
        {
            var archives = new List<IArchiveLoader>();
#if DEV
            var devPath = Path.Combine(".", "..", "..", "..", "..", "Core", "Mods");
            foreach (var path in Directory.GetDirectories(devPath))
            {
                if(!File.Exists(Path.Combine(path, "index.json")))
                {
                    Console.WriteLine("Detected invalid mod directory: "+ path);
                    continue;
                }
                archives.Add(loader.LoadDirectory(path));
            }
#else

            foreach (var path in Directory.GetDirectories(Path.Combine(".", "mods")))
            {
                if(!File.Exists(Path.Combine(path, "index.json")))
                {
                    Console.WriteLine("Detected invalid mod directory: "+ path);
                    continue;
                }
                archives.Add(loader.LoadDirectory(path));
            }
            
            foreach (var path in Directory.GetFiles(Path.Combine(".", "mods")))
            {
                if(!path.EndsWith(".zip"))
                {
                    Console.WriteLine("Detected invalid mod directory: "+ path);
                    continue;
                }
                archives.Add(loader.LoadArchive(path));
            }
#endif

            return archives;
        }
    }
}