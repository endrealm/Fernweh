using System;
using System.Collections.Generic;
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
            mods.Add(fileLoader.LoadDirectory("./../../../../Core/CoreMod"));
            if (modDir != null)
            {
                mods.Add(fileLoader.LoadArchive(modDir));
            }
            
            using (var game = new CoreGame(new MouseClickInput(),  mods))
                game.Run();
        }
    }
}