using System;
using System.IO;
using System.IO.Compression;
using Core;

namespace CrossPlatformDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CoreGame(new MouseClickInput(), new ContentLoader()))
                game.Run();
        }
    }
}