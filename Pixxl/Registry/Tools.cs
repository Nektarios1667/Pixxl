using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Reset", "Reset Temp", "Save", "Load", "Mode"};
        public static readonly Color[] Colors = { Color.DarkRed, Color.LightBlue, Color.Gold, Color.Azure, Color.Orchid};
        public static readonly Delegate[] Functions = { Reset.All, Reset.Temperature, State.Save, State.Load, Canvas.Mode };
    }
}
