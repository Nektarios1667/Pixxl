using System;
using Microsoft.Xna.Framework;

namespace Pixxl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Window())
            {
                game.Run();
            }
        }
    }
}