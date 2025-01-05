using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Constants
{
    public static class Screen
    {
        public static readonly int[] Window = [1600, 900];
        public static readonly int PixelSize = 5;
        public static readonly int[] Grid = [Window[0] / PixelSize, (Window[1] / PixelSize) - Gui.MenuSize];

    }
    public static class Gui
    {
        public static readonly Xna.Vector2 ButtonDim = new(100, 30);
        public static readonly int MenuSize = (int)(ButtonDim.Y * 3f) / Screen.PixelSize;
    }
    public static class Game
    {
        public static readonly int RoomTemp = 70;
        public static readonly float Gravity = 9.81f * Screen.PixelSize;
    }
    public static class Visual
    {
        public static readonly int ThermalMax = 500;
    }
}