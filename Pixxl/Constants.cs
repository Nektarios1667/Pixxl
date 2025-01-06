using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Constants
{
    public static class Screen
    {
        public static readonly int[] Window = [1600, 900];
        public const int PixelSize = 10;
        public static readonly int[] Grid = [Window[0] / PixelSize, (Window[1] / PixelSize) - Gui.MenuSize];

    }
    public static class Gui
    {
        public static readonly Xna.Vector2 ButtonDim = new(100, 30);
        public static readonly int MenuSize = (int)(ButtonDim.Y * 3f) / Screen.PixelSize;
    }
    public static class Game
    {
        public const int RoomTemp = 70;
        public const float Gravity = 9.81f * Screen.PixelSize;
        public const float Speed = 1f;
    }
    public static class Visual
    {
        public const int ThermalMax = 500;
    }
}