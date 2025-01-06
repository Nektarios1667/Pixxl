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
        public int ColorMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal
        public int Cycle = 0; // 10 tick cycle used for limiting calculations


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
                Button created = new(Batch, new(x, Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize)), Consts.Gui.ButtonDim, ToolReg.Names[i], Window.Font, Color.Black, ToolReg.Colors[i], Functions.Lighten(ToolReg.Colors[i], .2f), ToolReg.Functions[i], args: [this], borderColor:new(45, 45, 45));
                Buttons.Add(created);
            }

            // Selection
            for (int i = 0; i < MatReg.Names.Length; i++)
            {
                // 100x30
                float x = Consts.Gui.ButtonDim.X * (i % (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X));
                float y = Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize) + Consts.Gui.ButtonDim.Y * (float)Math.Floor((double)(i / (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X)) + 1);
                Button created = new(Batch, new(x, y), Consts.Gui.ButtonDim, MatReg.Names[i], Window.Font, Color.Black, MatReg.Colors[i], Functions.Lighten(MatReg.Colors[i], .2f), select, args: [MatReg.Names[i]]);
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
            for (int i = 0; i < Pixels.Length; i++) { Pixels[i].Update(); }

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
                created.Velocity = vel;
                return created;
            }
            return null;
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
        public static Color Faucet() => GetVariation(MatReg.Colors[MatReg.Id("Faucet")], 3); // WIP
        public static Color Coolant() => GetVariation(MatReg.Colors[MatReg.Id("Coolant")], 8);
        public static Color BlueFire() => GetVariation(MatReg.Colors[MatReg.Id("BlueFire")], 13);
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