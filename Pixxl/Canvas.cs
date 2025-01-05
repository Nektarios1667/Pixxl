using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Pixxl.Materials;
using Xna = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Pixxl.Gui;
using Microsoft.Xna.Framework.Input;
using MatReg = Pixxl.Registry.Materials;
using Pixxl.Tools;

namespace Pixxl
{
    public class Canvas
    {
        public Pixel[] Pixels { get; set; }
        public Window Window { get; set; }
        public List<Pixxl.Gui.Button> Buttons { get; set; }
        public int[] Size { get; set; }
        public SpriteBatch Batch { get; set; }
        public GraphicsDevice Device { get; set; }
        public Xna.Vector2 SizeVector = new(Const.PixelSize, Const.PixelSize);
        public float Delta { get; private set; }
        public Random Rand = new();
        public int ColorMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal
        public int Cycle = 0; // 10 tick cycle used for limiting calculations


        public Canvas(Window window, GraphicsDevice device, SpriteBatch batch)
        {
            Window = window;
            Size = Const.Grid;
            Pixels = new Pixel[Const.Grid[0] * Const.Grid[1]];
            Batch = batch;
            Device = device;
            void select(string selection) => Window.Selection = selection;

            // Create buttons
            Buttons = [];
            for (int i = 0; i < Registry.Materials.Names.Length; i++)
            {
                // 100x30
                float x = Const.ButtonDim.X * (i % (Const.Window[0] / Const.ButtonDim.X));
                float y = Const.Window[1] - (Const.PixelSize * Const.MenuSize) + Const.ButtonDim.Y * (float)Math.Floor((double)(i / (Const.Window[0] / Const.ButtonDim.X)));
                Button created = new(Batch, new(x, y), Const.ButtonDim, MatReg.Names[i], Window.Font, Color.Black, MatReg.Colors[i], Functions.Lighten(MatReg.Colors[i], .2f), select, args: MatReg.Names[i]);
                Buttons.Add(created);
            }

            // Fill pixels
            Pixels = Cleared(this);
        }

        // Updating
        public void Update(float delta, MouseState mouseState)
        {
            // Delta
            Delta = delta;
            // Pixels
            Pixel[] pixelsCopy = (Pixel[])Pixels.Clone();
            for (int i = 0; i < Pixels.Length; i++) { pixelsCopy[i].Update(); }

            // Buttons
            for (int i = 0; i < Buttons.Count; i++) { Buttons[i].Update(mouseState); }

            // Cycle
            Cycle = (Cycle + 1) % 10;
        }
        // Drawing
        public void Draw() {
            if (Batch != null)
            {
                // Pixels
                for (int i = 0; i < Pixels.Length; i++) { Pixels[i].Draw(); }

                // Buttons
                for (int i = 0; i < Buttons.Count; i++) { Buttons[i].Draw(); }
            } else
            {
                Console.WriteLine("Skipping drawing with uninitialized batch...");
            }
        }
        // Static
        public static Pixel[] Cleared(Canvas canvas)
        {
            Pixel[] pixels = new Pixel[Const.Grid[0] * Const.Grid[1]];
            for (int i = 0; i < Const.Grid[0] * Const.Grid[1]; i++)
            {
                pixels[i] = new Air(new Xna.Vector2((i % Const.Grid[0]) * Const.PixelSize, i / Const.Grid[0] * Const.PixelSize), canvas);
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

        public static Color Sand() => GetVariation(MatReg.Colors[MatReg.Id("Sand")], 18);
        public static Color Concrete() => GetVariation(MatReg.Colors[MatReg.Id("Concrete")], 9);
        public static Color Helium() => GetVariation(MatReg.Colors[MatReg.Id("Helium")], 12);
        public static Color Debug() => SelectColor([new(209, 42, 198), new(237, 47, 225), new(166, 18, 151), new(0, 0, 0)]);
        public static Color Water() => GetVariation(MatReg.Colors[MatReg.Id("Water")], 9);
        public static Color Glass() => GetVariation(MatReg.Colors[MatReg.Id("Glass")], 9);
        public static Color Ice() => GetVariation(MatReg.Colors[MatReg.Id("Ice")], 18);
        public static Color Lava() => GetVariation(MatReg.Colors[MatReg.Id("Lava")], 20);
        public static Color Plasma() => GetVariation(MatReg.Colors[MatReg.Id("Plasma")], 8);
        public static Color Steam() => GetVariation(MatReg.Colors[MatReg.Id("Steam")], 6);
        public static Color Fire() => GetVariation(MatReg.Colors[MatReg.Id("Fire")], 30);
        public static Color Copper() => GetVariation(MatReg.Colors[MatReg.Id("Copper")], 9);
        public static Color Insulation() => GetVariation(MatReg.Colors[MatReg.Id("Insulation")], 18);
    }
}

namespace Pixxl.Tools
{
    public static class Functions
    {
        public static Color Lighten(Color color, float percentage)
        {
            int r = color.R; int g = color.G; int b = color.B;
            r += (int)((255 - r) * percentage);
            g += (int)((255 - g) * percentage);
            b += (int)((255 - b) * percentage);
            return new(r, g, b);
        }
    }
}