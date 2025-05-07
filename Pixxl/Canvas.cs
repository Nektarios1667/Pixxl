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
using System.Linq;

namespace Pixxl
{
    public class Canvas
    {
        public Pixel[] Pixels { get; set; }
        public Window Window { get; set; }
        public List<Widget> Widgets { get; set; }
        public int[] Size { get; set; }
        public SpriteBatch Batch { get; set; }
        public GraphicsDevice Device { get; set; }
        public Xna.Vector2 SizeVector = new(Consts.Screen.PixelSize, Consts.Screen.PixelSize);
        public float Delta { get; private set; }
        public Random Rand = new();
        public int ViewMode = 0; // 0 = Textures, 1 = Colored thermal, 2 = B&W thermal, 3 = Monotexture
        public object? Focus { get; set; }
        public Popup SavesPopup { get; set; }
        public string Tab { get; set; }
        public Canvas(Window window, GraphicsDevice device, SpriteBatch batch)
        {
            Window = window;
            Focus = window;
            Size = Consts.Screen.Grid;
            Pixels = new Pixel[Consts.Screen.Grid[0] * Consts.Screen.Grid[1]];
            Batch = batch;
            Device = device;
            Tab = "Powder";

            // Saves
            int saveX = Consts.Screen.Window[0] / 2 - 400;
            SavesPopup = new(Batch, new(saveX, 100), new(400, 550), Color.LightCyan, "Saves", Window.Font);
            SavesPopup.Visible = false;
            for (int s = 0; s < 10; s++)
            {
                TextBox label = new(Batch, new(saveX + 20, s * 50 + 155), Color.Black, $"Slot {s + 1}", Window.Font);
                Button clearButton = new(Batch, new(saveX + 100, s * 50 + 150), new(80, 30), Color.Black, new(175, 175, 225), new(200, 200, 225), State.SavePixels, args: [Cleared(this), s + 1], font: Window.Font, text: "Clear");
                Button saveButton = new(Batch, new(saveX + 200, s * 50 + 150), new(80, 30), Color.Black, new(225, 175, 175), new(225, 200, 200), State.SaveCanvas, args: [this, s + 1], font: Window.Font, text: "Save");
                Button loadButton = new(Batch, new(saveX + 300, s * 50 + 150), new(80, 30), Color.Black, new(175, 225, 175), new(200, 225, 200), State.LoadCanvas, args: [this, s + 1], font: Window.Font, text: "Load");
                SavesPopup.AddWidgets(clearButton, saveButton, loadButton, label);
            }
            Widgets = [SavesPopup];
            CreateInterface();

            // Fill pixels
            Pixels = Cleared(this);
        }
        // Updating
        public void UpdatePixels(float delta)
        {
            // Delta
            Delta = delta;

            // Pixels
            Pixel[] copy = (Pixel[])(Pixels.Clone());
            for (int i = 0; i < copy.Length; i++) { copy[i].Update(); }
        }
        public void UpdateGui(float delta)
        {
            // Widgets
            Focus = Window;
            for (int i = 0; i < Widgets.Count; i++) {
                if (Widgets[i].GetType().Name == "Popup" && ((Popup)Widgets[i]).Visible) { Focus = Widgets[i]; }
                Widgets[i].Update(Window);
            }
        }
        // Drawing
        public void Draw() {
            if (Batch != null)
            {
                // Pixels
                for (int i = 0; i < Pixels.Length; i++) { Pixels[i].Draw(); }

                // Widgets
                for (int i = 0; i < Widgets.Count; i++) { Widgets[i].Draw(); }
            } else
            {
                Logger.Log("Skipping drawing with uninitialized batch...");
            }
        }
        public void CreateInterface()
        {

            void select(string selection) => Window.Selection = selection;

            // Create buttons
            Widgets = [Widgets[0]];

            // Tools buttons
            for (int t = 0; t < ToolReg.Names.Length; t++)
            {
                // Info
                float x = Consts.Gui.ToolDim.X * (t % (Consts.Screen.Window[0] / Consts.Gui.ToolDim.X));
                object? arg = ToolReg.Args[t][0] == "Canvas" ? this : ToolReg.Args[t][0] == "Window" ? Window : null;
                object?[]? args = ToolReg.Args[t].Length > 1 ? [arg, .. ToolReg.Args[t][1..]] : [arg];

                // Creation
                Button created = new(Batch, new(x, Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize)), Consts.Gui.ToolDim, Color.Black, ToolReg.Colors[t], Functions.Lighten(ToolReg.Colors[t], .2f), ToolReg.Functions[t], ToolReg.Names[t], Window.Font, args: args, borderColor: new(45, 45, 45));
                Widgets.Add(created);
            }

            // Materials buttons
            int l = 0; int m = 0;
            int infoboxY = Consts.Screen.Window[1] - Consts.Gui.MenuSize * Consts.Game.PixelSize - 44;
            for (m = 0; m < MatReg.Names.Count; m++)
            {
                if (MatReg.Names[m][0] == '.' && Tab != "Hidden") { continue; } // Skip hidden
                if (!MatReg.Tags[m].Contains(Tab) && !(Tab == "Hidden" && MatReg.Names[m][0] == '.')) { continue; } // Filter out based on tab selection
                // Button size, background, and foreground
                int x = (int)(Consts.Gui.ButtonDim.X * (l % (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X)));
                int y = (int)(Consts.Screen.Window[1] - (Consts.Screen.PixelSize * Consts.Gui.MenuSize) + Consts.Gui.ButtonDim.Y * (float)Math.Floor((double)(l / (Consts.Screen.Window[0] / Consts.Gui.ButtonDim.X)) + 1));
                Color bg = MatReg.Colors[m];
                int darkValues = 0; if (bg.R < 60) darkValues++; if (bg.G < 60) darkValues++; if (bg.B < 60) darkValues++; // 2/3 rgb values are dark
                bool prominentColor = bg.R > 165 || bg.G > 165 || bg.B > 165;
                Color fg = darkValues >= 2 && !prominentColor ? Color.White : Color.Black;

                // Button
                Button created = new(Batch, new(x, y), Consts.Gui.ButtonDim, fg, bg, Functions.Lighten(MatReg.Colors[m], .2f), select, MatReg.Names[m], Window.Font, args: [MatReg.Names[m]]);
                int infoboxX = x + 300 <= Consts.Screen.Window[0] ? (int)x : Consts.Screen.Window[0] - 300;
                Infobox infobox = new(Batch, new(infoboxX, infoboxY), new(300, 40), new((int)x, (int)y, (int)Consts.Gui.ButtonDim.X, (int)Consts.Gui.ButtonDim.Y), bg, fg, MatReg.Descriptions[m], Window.Font);
                Widgets.Add(created); Widgets.Add(infobox);
                l++;
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
            Type? typeObj = Type.GetType($"Pixxl.Materials.{(type[0] == '.' ? type[1..] : type)}");
            if (typeObj != null)
            {
                Pixel? created = (Pixel)Activator.CreateInstance(typeObj, loc, canvas);
                if (created == null) { return null; }

                if (temp != null) { created.Temperature = (float)temp; }
                return created;
            }
            return null;
        }
        public static void ChangeViewMode(Canvas canvas) { canvas.ViewMode = (canvas.ViewMode + 1) % 4; }
        public static void Saves(Canvas canvas) { canvas.SavesPopup.Visible = true; }
        public static void SetTab(Canvas canvas, string tab) { canvas.Tab = tab; canvas.CreateInterface(); }
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