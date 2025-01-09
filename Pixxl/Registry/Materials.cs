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
            Console.WriteLine("Loading Registry...");
            // Read file
            string[] lines;
            try
            {
                lines = File.ReadAllLines("Registry.txt").Where(line => !string.IsNullOrEmpty(line)).ToArray();
            } catch(FileNotFoundException e)
            {
                Console.WriteLine($"Error loading Registry - File not found '{e}'");
                return;
            }

            // Iterate lines
            foreach (string line in lines)
            {
                // Sections
                string[] sections = line.Split('|');

                // Checks
                if (sections.Length != 3)
                {
                    Console.WriteLine($"Error loading Registry - Invalid format '{line}'");
                    continue; // Skip invalid lines
                }

                // Names
                Names.Add(sections[0].Trim());

                // Colors
                string[] values = sections[1].Trim().Split(", ");
                if (!int.TryParse(values[0], out int r) || !int.TryParse(values[1], out int g) || !int.TryParse(values[2], out int b)) { Console.WriteLine($"Error loading Registry - Can not parse color for '{line}'"); continue; }
                Colors.Add(new(r, g, b));

                // Variations
                if (!int.TryParse(sections[2].Trim(), out int v)) { Console.WriteLine($"Error loading Registry - Can not parse color variation for '{line}'"); continue; }
                Variations.Add(v);
            }
            Console.WriteLine("Successfuly loaded Registry");
        }

        public static int Id(string material)
        {
            return Names.IndexOf(material);
        }
    }
}
