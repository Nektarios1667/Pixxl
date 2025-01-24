using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Reset", "Reset Temp", "Saves", "View Mode", "Erase", "Pause", "Run Frame", "Speed"};
        public static readonly Color[] Colors = { Color.DarkRed, Color.LightBlue, Color.Gold, Color.Azure, Color.Orchid, Color.Pink, Color.LemonChiffon, Color.MistyRose};
        public static readonly Delegate[] Functions = { Reset.All, Reset.Temperature, Canvas.Saves, Canvas.ChangeViewMode, Window.EraseMode, Window.TogglePlay, Window.RunFrame, Window.CycleSpeed};
        public static readonly string[] Args = { "Canvas", "Canvas", "Canvas", "Canvas", "Window", "Window", "Window", "Window"};
    }
}
