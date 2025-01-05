using Xna = Microsoft.Xna.Framework;

namespace Pixxl
{
    public static class Const
    {
        public static readonly int RoomTemp = 70;
        public static readonly int[] Window = [1200, 900];
        public static readonly int PixelSize = 5;
        public static readonly float Gravity = 9.81f * PixelSize;

        public static readonly Xna.Vector2 ButtonDim = new(100, 30);
        public static readonly int MenuSize = (int)(ButtonDim.Y * 3f) / PixelSize;
        public static readonly int[] Grid = [Window[0] / PixelSize, (Window[1] / PixelSize) - MenuSize];

        public static readonly int ThermalMax = 500;
    }
}