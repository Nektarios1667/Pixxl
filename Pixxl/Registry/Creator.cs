using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pixxl.MaterialCreator
{
    public class Creator
    {
        public static void Main2(string[] args)
        {
            string[] lines = File.ReadAllLines("Creations.txt");

            int l = 0;
            foreach (string line in lines)
            {
                // Commented
                if (line.StartsWith("//")) { continue; }

                // Collect and create
                string[] parts = line.Split(" | ");
                string classText = $"using System;\r\nusing Xna = Microsoft.Xna.Framework;\r\nusing MonoGame.Extended;\r\nusing Microsoft.Xna.Framework;\r\n\r\nnamespace Pixxl.Materials\r\n{{\r\n    public class {parts[0]} : Pixel\r\n    {{\r\n        // Constructor\r\n        public {parts[0]}(Xna.Vector2 location, Canvas canvas) : base(location, canvas)\r\n        {{\r\n            // Constants\r\n            Conductivity = {parts[5]}f;\r\n            Density = {parts[6]}f;\r\n            State = {parts[7]};\r\n            Strength = {parts[8]};\r\n            Melting = new Transformation({parts[9]}, typeof({parts[10]}));\r\n            Solidifying = new Transformation({parts[11]}, typeof({parts[12]}));\r\n{(parts[13] == "false" ? "            Gravity = false;\r\n" : "")}        }}\r\n    }}\r\n}}\r\n";

                // Write class file
                File.WriteAllText($"C:\\Users\\nekta\\source\\repos\\Pixxl\\Pixxl\\Materials\\{parts[0]}.cs", classText);

                // Registry
                List<string> filedLines = File.ReadAllLines("C:\\Users\\nekta\\source\\repos\\Pixxl\\Pixxl\\Registry\\Filed.cs").ToList();
                List<string> output = [];
                int f = 0;
                bool addRepl = false;
                string replacement = "";
                foreach (string filedLine in filedLines)
                {
                    addRepl = false;
                    // Quotation lines
                    if (filedLine.Contains('|') && f >= 13)
                    {
                        // Trim
                        string name = filedLine.Trim();
                        if (name[0] == '.') { name = name[1..]; }
                        name = name.Split('|')[0].Trim();

                        // Compare
                        replacement = $"            {parts[0],-14} | {parts[1],-13} | {parts[2],-2} | {parts[3],-20} | {parts[4]}";
                        if (f == 0 && parts[0].CompareTo(name) < 0) { output.Insert(0, replacement); }
                        else if (f == output.Count - 1 && parts[0].CompareTo(name) > 0) { output.Add(replacement); }
                        else if (parts[0].CompareTo(name) > 0 && parts[0].CompareTo(filedLines[f + 1].Trim().Trim('.').Split('|')[0].Trim()) < 0)
                        {
                            addRepl = true; 
                        }
                    }

                    // Reconstruct
                    output.Add(filedLine);
                    if (addRepl) { output.Add(replacement); }
                    f++;
                }

                // Write
                File.WriteAllLines("C:\\Users\\nekta\\source\\repos\\Pixxl\\Pixxl\\Registry\\Filed.cs", output);
                l++;
            }
        }
    }
}
