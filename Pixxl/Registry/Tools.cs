using System;
using Microsoft.Xna.Framework;
using Pixxl.Tools;

namespace Pixxl.Registry
{
    public static class Tools
    {
        public static readonly string[] Names = {
            "Reset",
            "Replace",
            "Saves",
            "View Mode",
            "Erase",
            "Pause",
            "Step",
            "Speed",
            "Solids",
            "Powders",
            "Liquids",
            "Gases",
            "Energy",
            "Natural",
            "Explosives",
            "Hidden"
        };
        public static readonly Color[] Colors = {
            Color.Red,            // Reset
            Color.Orange,         // Reset Temp
            Color.LightGray,      // Saves
            Color.MediumPurple,   // View Mode
            Color.LightPink,      // Erase
            Color.Yellow,         // Pause
            Color.LawnGreen,      // Step
            Color.Silver,         // Speed
            Color.SaddleBrown,    // Solids
            Color.Goldenrod,      // Powders
            Color.DodgerBlue,     // Liquids
            Color.LightSkyBlue,   // Gases
            Color.OrangeRed,      // Energy
            Color.ForestGreen,    // Natural
            Color.Crimson,        // Explosives
            Color.DarkSlateGray   // Hidden
        }; 
        public static readonly Delegate[] Functions = {
            Reset.All,             // Reset
            Window.ToggleReplace,  // Reset Temp 
            Canvas.Saves,          // Saves
            Canvas.ChangeViewMode, // View Mode
            Window.EraseMode,      // Erase
            Window.TogglePlay,     // Pause
            Window.RunFrame,       // Step
            Window.CycleSpeed,     // Speed
            Canvas.SetTab,         // Solids
            Canvas.SetTab,         // Powders
            Canvas.SetTab,         // Liquids
            Canvas.SetTab,         // Gases
            Canvas.SetTab,         // Energy
            Canvas.SetTab,         // Natural
            Canvas.SetTab,         // Explosives
            Canvas.SetTab          // Hidden
        };
        public static readonly string[][] Args = {
            ["Canvas"],              // Reset
            ["Window"],              // Reset Temp
            ["Canvas"],              // Saves
            ["Canvas"],              // View Mode
            ["Window"],              // Erase
            ["Window"],              // Pause
            ["Window"],              // Step
            ["Window"],              // Speed
            ["Canvas", "Solid"],     // Solids
            ["Canvas", "Powder"],    // Powders
            ["Canvas", "Liquid"],    // Liquids
            ["Canvas", "Gas"],       // Gases
            ["Canvas", "Energy"],    // Energy
            ["Canvas", "Natural"],   // Natural
            ["Canvas", "Explosive"], // Explosives
            ["Canvas", "Hidden"]     // Hidden
        };
    }
}
