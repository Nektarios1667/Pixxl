using System;
using Microsoft.CodeAnalysis.Operations;

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