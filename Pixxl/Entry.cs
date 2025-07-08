global using System;
global using Pixxl.Gui;

global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;


namespace Pixxl
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Window())
            {
                try { game.Run(); }
                catch (Exception ex) { Logger.Log($"Exception occured: {ex}"); throw; }
            }
        }
    }
}