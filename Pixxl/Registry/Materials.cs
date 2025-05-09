using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

// TODO Add a bunch of materials for each category
// Solid: brass
// Powder: salt, 
// Liquid: molten brass
// Gas:
// Energy: lighting?
namespace Pixxl.Registry
{
    public static class Materials
    {
        public static readonly List<string> Names = [];
        public static readonly List<Color> Colors = [];
        public static readonly List<int> Variations = [];
        public static readonly List<string> Descriptions = [];
        public static readonly List<string[]> Tags = [];

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
                    Tags.Add(reg.Value.Tags);
                    Descriptions.Add(reg.Value.Description);

                }
            }
            Logger.Log("Successfuly loaded Registry");
        }
        public static Registered? Register(string line)
        {
            // Sections
            string[] sections = line.Split('|');

            // Checks
            if (sections.Length != 5)
            {
                Logger.Log($"Error loading Registry - Invalid format '{line}'");
                return null; // Skip invalid lines
            }

            // Colors
            string[] values = sections[1].Trim().Split(", ");
            if (!int.TryParse(values[0], out int r) || !int.TryParse(values[1], out int g) || !int.TryParse(values[2], out int b)) { Logger.Log($"Error loading Registry - Can not parse color for '{line}'"); return null; }

            // Variations
            if (!int.TryParse(sections[2].Trim(), out int variation)) { Logger.Log($"Error loading Registry - Can not parse color variation for '{line}'"); return null; }

            // Tags
            string tags = sections[3].Trim();

            // Description
            string description = sections[4].Trim();

            // Log
            Logger.Log($"Loaded material '{sections[0].Trim()}' to Register");
            return new(sections[0].Trim(), new Color(r, g, b), variation, tags, description);
        }

        public struct Registered
        {
            public string Name { get; set; }
            public Color Color { get; set; }
            public int Variation { get; set; }
            public string[] Tags { get; set; }
            public string Description { get; set; }

            public Registered(string name, Color color, int variation, string tags, string description)
            {
                Name = name;
                Color = color;
                Variation = variation;
                Tags = tags.Split(';');
                Description = description;
            }
        }

        public static int Id(string material)
        {
            // Return index of material if its found and if not try finding a hidden material with that name
            return Names.IndexOf(material) is var idx && idx != -1 ? idx : Names.IndexOf($".{material}");
        }
    }
}
