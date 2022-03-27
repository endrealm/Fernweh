using System;
using System.IO;
using System.IO.Compression;
using Core;

namespace CrossPlatformDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var modDir = args.Length > 0 ? args[0] : null;

            using (var game = new CoreGame(new MouseClickInput(), new FileLoader(), "./../../../../Core/content.zip", modDir))
                game.Run();
        }
    }
}