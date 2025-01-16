using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Pixxl.Materials;
using Xna = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework;
using Pixxl.Gui;
using Microsoft.Xna.Framework.Input;
using MatReg = Pixxl.Registry.Materials;
using ToolReg = Pixxl.Registry.Tools;
using Pixxl.Tools;
using Consts = Pixxl.Constants;

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
        public Xna.Vector2 SizeVector = new(Consts.Screen.PixelSize, Consts.Screen.PixelSize);
        public float Delta { get; private set; }
        public Random Rand = new();
        public int ColorMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal, 3 = Monotexture


        public Canvas(Window window, GraphicsDevice device, SpriteBatch batch)
        {
            Window = window;
            Size = Consts.Screen.Grid;
            Pixels = new Pixel[Consts.Screen.Grid[0] * Consts.Screen.Grid[1]];
            Batch = batch;
            Device = device;
            void select(string selection) => Window.Selection = selection;

            // Create buttons
            Buttons = [];

            // Tools
            for (int i = 0; i < ToolReg.Names.Length; i++)
            {
                float x = Consts.Gui.ButtonDim.X * (i % (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X));
                Button created = new(Batch, new(x, Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize)), Consts.Gui.ButtonDim, ToolReg.Names[i], Window.Font, Color.Black, ToolReg.Colors[i], Functions.Lighten(ToolReg.Colors[i], .2f), ToolReg.Functions[i], args: [this], borderColor: new(45, 45, 45));
                Buttons.Add(created);
            }

            // Selection
            for (int i = 0; i < MatReg.Names.Count; i++)
            {
                // Button size, background, and foreground
                float x = Consts.Gui.ButtonDim.X * (i % (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X));
                float y = Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize) + Consts.Gui.ButtonDim.Y * (float)Math.Floor((double)(i / (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X)) + 1);
                Color bg = MatReg.Colors[i];
                int darkValues = 0; if (bg.R < 50) darkValues++; if (bg.G < 50) darkValues++; if (bg.B < 50) darkValues++; // 2/3 rgb values are dark
                Color fg = darkValues >= 2 ? Color.White : Color.Black;

                // Button
                Button created = new(Batch, new(x, y), Consts.Gui.ButtonDim, MatReg.Names[i], Window.Font, fg, bg, Functions.Lighten(MatReg.Colors[i], .2f), select, args: [MatReg.Names[i]]);
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
            Pixel[] copy = (Pixel[])(Pixels.Clone());
            for (int i = 0; i < copy.Length; i++) { copy[i].Update(); }

            // Buttons
            for (int i = 0; i < Buttons.Count; i++) { Buttons[i].Update(mouseState); }
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
                Logger.Log("Skipping drawing with uninitialized batch...");
            }
        }
        // Static
        public static Pixel[] Cleared(Canvas canvas)
        {
            int[] grid = Consts.Screen.Grid;
            Pixel[] pixels = new Pixel[grid[0] * grid[1]];
            for (int i = 0; i < Consts.Screen.Grid[0] * grid[1]; i++)
            {
                pixels[i] = new Air(new Xna.Vector2((i % grid[0]) * Consts.Screen.PixelSize, i / Consts.Screen.Grid[0] * Consts.Screen.PixelSize), canvas);
            }
            return pixels;
        }
        public static Pixel? New(Canvas canvas, string type, Xna.Vector2 loc, float? temp = null, float vel = 0)
        {
            Type? typeObj = Type.GetType($"Pixxl.Materials.{type}");
            if (typeObj != null)
            {
                Pixel? created = (Pixel)Activator.CreateInstance(typeObj, loc, canvas);
                if (created == null) { return null; }

                if (temp != null) { created.Temperature = (float)temp; }
                return created;
            }
            return null;
        }
        public static void Mode(Canvas canvas)
        {
            canvas.ColorMode = (canvas.ColorMode + 1) % 4;
        }
    }
}

namespace Pixxl
{
    public static class ColorSchemes
    {
        static readonly Random rand = new();
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

        public static Color GetColor(int id) { return GetVariation(MatReg.Colors[id], MatReg.Variations[id]); }
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