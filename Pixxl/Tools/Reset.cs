using Pixxl.Materials;

namespace Pixxl.Tools
{
    public static class Reset
    {
        public static void Temperature(Canvas canvas)
        {
            Pixel[] pixels = canvas.Pixels;
            for (int i = 0; i < pixels.Length; i++) { pixels[i].Temperature = Constants.Game.RoomTemp; }
            canvas.NextPixels = canvas.Pixels;
            Logger.Log("Cleared temperature");
        }
        public static void All(Canvas canvas)
        {
            canvas.Pixels = Canvas.Cleared(canvas);
            canvas.NextPixels = canvas.Pixels;
            Logger.Log("Cleared canvas");
        }
    }
}
