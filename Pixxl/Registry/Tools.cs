using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = { "Reset", "Reset Temp", "Saves", "View Mode", "Erase", "Pause", "Step", "Speed", "Solids", "Powders", "Liquids", "Gases", "Energy", "Natural", "Metals", "Explosives", "Spawners", "Hidden"};
        public static readonly Color[] Colors = { Color.DarkRed, Color.LightBlue, Color.Gold, Color.Azure, Color.Orchid, Color.Pink, Color.LemonChiffon, Color.MistyRose, Color.DarkGoldenrod, Color.NavajoWhite, Color.SaddleBrown, Color.SeaShell, Color.Tomato, Color.WhiteSmoke, Color.LightGray, Color.LightSeaGreen, Color.GhostWhite, Color.MediumAquamarine};
        public static readonly Delegate[] Functions = { Reset.All, Reset.Temperature, Canvas.Saves, Canvas.ChangeViewMode, Window.EraseMode, Window.TogglePlay, Window.RunFrame, Window.CycleSpeed, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab, Canvas.SetTab};
        public static readonly string[][] Args = { ["Canvas"], ["Canvas"], ["Canvas"], ["Canvas"], ["Window"], ["Window"], ["Window"], ["Window"], ["Canvas", "Solid"], ["Canvas", "Powder"], ["Canvas", "Liquid"], ["Canvas", "Gas"], ["Canvas", "Energy"], ["Canvas", "Natural"], ["Canvas", "Metal"], ["Canvas", "Explosive"], ["Canvas", "Indestructable"], ["Canvas", "Hidden"] };
    }
}
