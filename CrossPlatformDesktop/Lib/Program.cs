using System;
using Core;

namespace CrossPlatformDesktop
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CoreGame(new MouseClickInput()))
                game.Run();
        }
    }
}