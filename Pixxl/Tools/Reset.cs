using System;
using Pixxl.Materials;

namespace Pixxl.Tools
{
    public static class Reset
    {
        public static void Temperature(ref Pixel[] pixels, float temp)
        {
            for (int i = 0; i < pixels.Length; i++) { pixels[i].Temperature = temp; }
        }
        public static void Temperature(ref Pixel[] pixels, int temp)
        {
            for (int i = 0; i < pixels.Length; i++) { pixels[i].Temperature = (float)temp; }
        }
        public static void All(Canvas canvas)
        {
            canvas.Pixels = Canvas.Cleared(canvas);
        }
    }
}
