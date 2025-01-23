using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Reset", "Reset Temp", "Save", "Load", "View Mode", "Erase", "Pause", "Run Frame", "Speed"};
        public static readonly Color[] Colors = { Color.DarkRed, Color.LightBlue, Color.Gold, Color.Azure, Color.Orchid, Color.Pink, Color.LemonChiffon, Color.Bisque, Color.MistyRose};
        public static readonly Delegate[] Functions = { Reset.All, Reset.Temperature, State.Save, State.Load, Canvas.ChangeViewMode, Window.EraseMode, Window.TogglePlay, Window.RunFrame, Window.SpeedUp};
        public static readonly string[] Args = { "Canvas", "Canvas", "Canvas", "Canvas", "Canvas", "Window", "Window", "Window", "Window"};
    }
}
