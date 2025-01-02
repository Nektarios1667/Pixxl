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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;

namespace Pixxl
{
    public class Canvas
    {
        public Pixel[,] Pixels { get; set; }
        public int[] Size { get; set; }
        public SpriteBatch Batch { get; set; }
        public GraphicsDevice Device { get; set; }
        public Xna.Vector2 SizeVector = new(Const.PixelSize, Const.PixelSize);
        public float Delta { get; private set; }
        public Random Rand = new();
        public int ColorMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal

        public Canvas(GraphicsDevice device, SpriteBatch batch)
        {
            Size = Const.Grid;
            Pixels = new Pixel[Const.Grid[0], Const.Grid[1]];
            Batch = batch;
            Device = device;

            // Fill pixels
            Pixels = Cleared(this);
        }

        // Updating
        public void Update(float delta)
        {
            Delta = delta;
            for (int y = 0; y < Const.Grid[1]; y++)  // Loop through rows
            {
                for (int x = 0; x < Const.Grid[0]; x++)  // Loop through columns
                {
                    Pixels[y, x].Update();
                }
            }
        }
        // Drawing
        public void Draw() {
            if (Batch != null)
            {
                // Render setup
                RenderTarget2D render = new(Device, 1200, 900);
                Device.SetRenderTarget(render);

                // Drawing
                for (int y = 0; y < Const.Grid[1]; y++)  // Loop through rows
                {
                    for (int x = 0; x < Const.Grid[0]; x++)  // Loop through columns
                    {
                        Pixels[y, x].Draw();
                    }
                }

                // Paste render
                Batch.End();
                Batch.Begin();
                Device.SetRenderTarget(null);
                Batch.Draw(render, Xna.Vector2.Zero, Color.White);

            } else
            {
                Console.WriteLine("Skipping drawing with uninitialized batch...");
            }
        }
        // Static
        public static Pixel[,] Cleared(Canvas canvas)
        {
            Pixel[,] pixels = new Pixel[Const.Grid[1], Const.Grid[0]];

            for (int y = 0; y < Const.Grid[1]; y++)  // Loop through rows
            {
                for (int x = 0; x < Const.Grid[0]; x++)  // Loop through columns
                {
                    pixels[y, x] = new Air(new Xna.Vector2(x * Const.PixelSize, y * Const.PixelSize), canvas);
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
            return new Color(Math.Clamp(r, 0, 255), Math.Clamp(g, 0, 255), Math.Clamp(b, 0, 255));
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
        public static Color Fire() => GetVariation(new(189, 46, 21), 12);
    }
}