using System;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Constants
{
    public static class Screen
    {
        public const int PixelSize = Game.PixelSize;
        public static readonly int[] Window = [1600, 900];
        public static readonly int[] Grid = [Window[0] / PixelSize, (Window[1] / PixelSize) - Gui.MenuSize];
        public static readonly int[] Drawing = [Window[0], Window[1] - Gui.MenuSize * PixelSize];

    }
    public static class Gui
    {
        public static readonly Xna.Vector2 ButtonDim = new(100, 30);
        public static readonly Xna.Vector2 ToolDim = new(100, 30);
        public static readonly int MenuSize = (int)Math.Max(ButtonDim.Y * 3f, Screen.PixelSize) / Screen.PixelSize;
    }
    public static class Game
    {
        public const int PixelSize = 8;
        public const int RoomTemp = 70;
        public const float HeatTransfer = 1f;
        public const float Gravity = 9.81f * Screen.PixelSize;
        public static float Speed = 1f;
        public const bool Diagonals = true;
    }
    public static class Visual
    {
        public const int ThermalMax = 1000;
        public const int FeedLength = 8;
    }
}