using System;
using Microsoft.Xna.Framework;

namespace Pixxl.Registry
{
    public static class Materials
    {
        public static readonly string[] Names = { "Air", "BlueFire", "Concrete", "Coolant", "Copper", "Explosive", "#####", "Fire", "Glass", "Helium", "Ice", "Insulation", "Lava", "Plasma", "Sand", "Steam", "Water" };
        public static readonly Color[] Colors = { Color.CornflowerBlue, new(11, 106, 230), new(169, 169, 169), new(31, 181, 111), new(173, 86, 31), Color.DarkRed, new(0, 0, 155), new(189, 46, 21), new(190, 222, 232), new(168, 213, 227), new(130, 199, 245), new(245, 245, 245), new(201, 67, 26), new(187, 57, 227), new(186, 194, 33), new(191, 191, 191), new(0, 77, 207) };
        public static int Id(string material)
        {
            return Array.IndexOf(Names, material);
        }
    }
}
