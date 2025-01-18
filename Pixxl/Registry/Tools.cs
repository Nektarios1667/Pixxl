using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Reset", "Reset Temp", "Save", "Load", "Mode", "Erase"};
        public static readonly Color[] Colors = { Color.DarkRed, Color.LightBlue, Color.Gold, Color.Azure, Color.Orchid, Color.Pink};
        public static readonly Delegate[] Functions = { Reset.All, Reset.Temperature, State.Save, State.Load, Canvas.Mode, Window.EraseMode };
        public static readonly string[] Args = { "Canvas", "Canvas", "Canvas", "Canvas", "Canvas", "Window" };
    }
}
