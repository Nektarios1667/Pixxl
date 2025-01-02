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
            Delta = .0167f;
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
                Batch.Begin();
                foreach (List<Pixel> row in Pixels)
                {
                    foreach (Pixel pix in row)
                    {
                        pix.Draw();
                    }
                }
                Batch.End();
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
        static Random rand = new();
        // Sand
        public static Color Sand()
        {
            Color[] scheme = { new(186, 194, 33), new(230, 219, 25), new(202, 208, 51), new(218, 207, 28), new(204, 211, 41), new(221, 214, 37) };
            return scheme[rand.Next(0, scheme.Length)];
        }
        // Concrete
        public static Color Concrete()
        {
            Color[] scheme = { new(169, 169, 169), new(192, 192, 192), new(128, 128, 128), new(105, 105, 105), new(211, 211, 211) };
            return scheme[rand.Next(0, scheme.Length)];
        }
        // Helium
        public static Color Helium()
        {
            Color[] scheme = { new(173, 216, 230), new(176, 224, 230), new(170, 240, 250), new(190, 225, 245), new(180, 210, 230) };
            return scheme[rand.Next(0, scheme.Length)];
        }
    }
}