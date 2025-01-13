using System;
using Pixxl.Materials;

namespace Pixxl.Tools
{
    public static class Reset
    {
        public static void Temperature(Canvas canvas)
        {
            Pixel[] pixels = canvas.Pixels;
            Logger.Log("Cleared temperature");
            for (int i = 0; i < pixels.Length; i++) { pixels[i].Temperature = Constants.Game.RoomTemp; }
        }
        public static void All(Canvas canvas)
        {
            Logger.Log("Cleared canvas");
            canvas.Pixels = Canvas.Cleared(canvas);
        }
    }
}
