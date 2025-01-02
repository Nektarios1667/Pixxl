using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Pixxl.Materials;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl
{
    public class Canvas
    {
        public List<List<Pixel>> Pixels { get; set; }
        public int[] Size { get; set; }
        public SpriteBatch Batch { get; set; }
        public Xna.Vector2 SizeVector = new(Const.PixelSize, Const.PixelSize);
        public float Delta { get; private set; }
        public Random Rand = new();
        public int ColorMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal

        public Canvas(SpriteBatch batch)
        {
            Size = Const.Grid;
            Pixels = new();
            Batch = batch;

            // Fill pixels
            Pixels = Cleared(this);
        }

        // Updating
        public void Update(float delta)
        {
            Delta = delta;
            foreach (List<Pixel> row in Pixels.ToArray())
            {
                foreach (Pixel pix in row.ToArray())
                {
                    pix.Update();
                }
            }
        }
        // Drawing
        public void Draw() {
            if (Batch != null)
            {
                foreach (List<Pixel> row in Pixels)
                {
                    foreach (Pixel pix in row)
                    {
                        pix.Draw();
                    }
                }
            } else
            {
                Console.WriteLine("Skipping drawing with uninitialized batch...");
            }
        }
        // Static
        public static List<List<Pixel>> Cleared(Canvas canvas)
        {
            List<List<Pixel>> pixels = new();

            for (int y = 0; y < Const.Grid[1]; y++)  // Loop through rows
            {
                pixels.Add(new());
                for (int x = 0; x < Const.Grid[0]; x++)  // Loop through columns
                {
                    pixels[y].Add(new Air(new Xna.Vector2(x * Const.PixelSize, y * Const.PixelSize), canvas));
                }
            }
            return pixels;
        }
    }
}

namespace Pixxl
{
    public static class ColorSchemes
    {
        static readonly Random rand = new();
        public static Color SelectColor(Color[] colors)
        {
            return colors[rand.Next(0, colors.Length)];
        }
        public static Color GetVariation(Color color, int range)
        {
            // Break down
            int r = color.R; int g = color.G; int b = color.B;
            // Randomize
            int shift = rand.Next(-range, range + 1);
            r += shift; g += shift; b += shift;
            // Return
            return new Color(r, g, b);
        }

        public static Color Sand() => GetVariation(new(186, 194, 33), 18);
        public static Color Concrete() => GetVariation(new(169, 169, 169), 9);
        public static Color Helium() => GetVariation(new(168, 213, 227), 12);
        public static Color Debug() => SelectColor([new(209, 42, 198), new(237, 47, 225), new(166, 18, 151), new(0, 0, 0)]);
        public static Color Water() => GetVariation(new(0, 77, 207), 9);
        public static Color Glass() => GetVariation(new(190, 222, 232), 9);
        public static Color Ice() => GetVariation(new(130, 199, 245), 18);
        public static Color Lava() => GetVariation(new(201, 67, 26), 20);
        public static Color Plasma() => GetVariation(new(187, 57, 227), 8);
        public static Color Steam() => GetVariation(new(191, 191, 191), 6);
    }
}