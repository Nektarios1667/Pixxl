using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pixxl.Registry
{
    public static class Materials
    {
        public static readonly List<string> Names = new();
        public static readonly List<Color> Colors = new();
        public static readonly List<int> Variations = new();

        static Materials()
        {
            Logger.Log("Loading Registry...");
            // Read file
            string[] lines;
            lines = Filed.Materials.Split('\n').Where(line => !string.IsNullOrEmpty(line)).ToArray();

            // Iterate lines
            foreach (string line in lines)
            {
                Registered? reg = Register(line);
                if (reg != null)
                {
                    Names.Add(reg.Value.Name);
                    Colors.Add(reg.Value.Color);
                    Variations.Add(reg.Value.Variation);
                }
            }
            Logger.Log("Successfuly loaded Registry");
        }
        public static Registered? Register(string line)
        {
            // Sections
            string[] sections = line.Split('|');

            // Checks
            if (sections.Length != 3)
            {
                Logger.Log($"Error loading Registry - Invalid format '{line}'");
                return null; // Skip invalid lines
            }


            // Colors
            string[] values = sections[1].Trim().Split(", ");
            if (!int.TryParse(values[0], out int r) || !int.TryParse(values[1], out int g) || !int.TryParse(values[2], out int b)) { Logger.Log($"Error loading Registry - Can not parse color for '{line}'"); return null; }

            // Variations
            if (!int.TryParse(sections[2].Trim(), out int variation)) { Logger.Log($"Error loading Registry - Can not parse color variation for '{line}'"); return null; }

            // Log
            Logger.Log($"Loaded material '{sections[0].Trim()}' to Register");
            return new(sections[0].Trim(), new Color(r, g, b), variation);
        }

        public struct Registered
        {
            public string Name { get; set; }
            public Color Color { get; set; }
            public int Variation { get; set; }

            public Registered(string name, Color color, int variation)
            {
                Name = name;
                Color = color;
                Variation = variation;
            }
        }

        public static int Id(string material)
        {
            // Return index of material if its found and if not try finding a hidden material with that name
            return Names.IndexOf(material) is var idx && idx != -1 ? idx : Names.IndexOf($".{material}");
        }
    }
}
