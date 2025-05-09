using System.IO;

namespace Pixxl.MaterialCreator
{
    public class Creator
    {
        public static void Main2(string[] args)
        {
            string[] lines = File.ReadAllLines("C:\\Users\\nekta\\source\\repos\\Pixxl\\Pixxl\\Registry\\Creations.txt");

            foreach (string line in lines)
            {
                // Commented
                if (line.StartsWith("//")) { continue; }

                // Collect and create
                string[] parts = line.Split(" | ");
                string classText = $"using Xna = Microsoft.Xna.Framework;\r\n\r\nnamespace Pixxl.Materials\r\n{{\r\n    public class {parts[0]} : Pixel\r\n    {{\r\n        // Constructor\r\n        public {parts[0]}(Xna.Vector2 location, Canvas canvas) : base(location, canvas)\r\n        {{\r\n            // Constants\r\n            Conductivity = {parts[1]}f;\r\n            Density = {parts[2]}f;\r\n            State = {parts[3]};\r\n            Strength = {parts[4]};\r\n            Melting = new Transformation({parts[5]}, typeof({parts[6]}));\r\n            Solidifying = new Transformation({parts[7]}, typeof({parts[8]}));\r\n{(parts[9] == "false" ? "            Gravity = false;\r\n" : "")}        }}\r\n    }}\r\n}}\r\n";

                // Write class file
                File.WriteAllText($"C:\\Users\\nekta\\source\\repos\\Pixxl\\Pixxl\\Materials\\{parts[0]}.cs", classText);
            }
        }
    }
}
