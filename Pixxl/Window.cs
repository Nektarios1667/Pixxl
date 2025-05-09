using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Xna = Microsoft.Xna.Framework;
using Pixxl.Materials;
using Consts = Pixxl.Constants;
using System.Collections.Generic;
using System.Runtime;
using MonoGame.Extended;
using Pixxl.Gui;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pixxl
{
    public class Window : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public Canvas canvas;
        public Xna.Vector2 coord;
        public SpriteFont Font;
        public SpriteFont SmallFont;
        public string Selection;
        public Xna.Vector2 location = new(0, 0);
        public int Running = 2; // 0 = paused; 1 = one frame; 2 = running
        public float Delta = 0;
        public string[] ColorModes = ["Regular", "Colored Thermal", "Grayscale Thermal", "Monotexture"];
        public int CursorSize = 1;
        public bool Replace = false;

        private Keys[] previous = [];
        public Keys[] keys = [];
        public MouseState mouse;
        public MouseState previousMouse;
        private string cursor;

        public Window()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Consts.Screen.Window[0],
                PreferredBackBufferHeight = Consts.Screen.Window[1]
            };
            IsMouseVisible = false;
            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";
            Selection = "Sand";
            cursor = Selection;

            Logger.Log("Initialized window");
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Arial");
            SmallFont = Content.Load<SpriteFont>("ArialSmall");

            // Load canvas at the end
            canvas = new(this, graphics.GraphicsDevice, spriteBatch);
            Logger.Log("Loaded content");

            previousMouse = new();
        }

        protected override void Update(GameTime gameTime)
        {
            // States
            keys = Keyboard.GetState().GetPressedKeys();
            mouse = Mouse.GetState();
            location = mouse.Position.ToVector2();
            coord = new((int)Math.Floor((float)location.X / Consts.Screen.PixelSize), (int)Math.Floor((float)location.Y / Consts.Screen.PixelSize));
            Delta = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            // Exit
            if (keys.Contains(Keys.Escape))
                Exit();

            // Drawing
            if (canvas.Focus == this)
            {
                if (mouse.LeftButton == ButtonState.Pressed && Inside(coord, Consts.Screen.Grid))
                {
                    cursor = Selection;
                    foreach (Pixel pixel in canvas.Pixels)
                    {
                        if ((pixel.Type == "Air" || (Replace && pixel.Type != cursor)) && Math.Abs(pixel.Coords.X - coord.X) < CursorSize && Math.Abs(pixel.Coords.Y - coord.Y) < CursorSize)
                        {
                            canvas.Pixels[pixel.Index] = Canvas.New(canvas, Selection, pixel.Location);
                        }
                    }
                }
                else if (mouse.RightButton == ButtonState.Pressed && Inside(coord, Consts.Screen.Grid))
                {
                    cursor = "Erase";
                    foreach (Pixel pixel in canvas.Pixels)
                    {
                        if ((pixel.Type != "Air" || Replace) && Math.Abs(pixel.Coords.X - coord.X) < CursorSize && Math.Abs(pixel.Coords.Y - coord.Y) < CursorSize)
                        {
                            canvas.Pixels[pixel.Index] = new Air(pixel.Location, canvas);
                        }
                    }
                }
                else if (mouse.MiddleButton == ButtonState.Pressed && Inside(coord, Consts.Screen.Grid))
                {
                    string picked = canvas.Pixels[Pixel.Flat(coord)].Type;
                    Selection = picked != "Air" ? picked : Selection;
                    cursor = Selection;
                }
                else { cursor = "Draw"; }

            } else { cursor = "Normal"; }

            // Hotkeys
            if (canvas.Focus == this)
            {
                if (KeyPress(Keys.V)) { Canvas.ChangeViewMode(canvas); }
                if (KeyPress(Keys.E)) { EraseMode(this); }
                if (KeyPress(Keys.P)) { TogglePlay(this); }
                if (KeyPress(Keys.F)) { RunFrame(this); }
                if (KeyPress(Keys.D)) { CycleSpeed(this); }
                if (KeyPress(Keys.R)) { Replace = !Replace; }
            }

            previous = keys.ToArray();

            // Change cursor size
            int scrollDelta = mouse.ScrollWheelValue - previousMouse.ScrollWheelValue;
            if (scrollDelta > 0) { CursorSize = Math.Min(CursorSize + 1, 5); }
            if (scrollDelta < 0) { CursorSize = Math.Max(CursorSize - 1, 1); }

            // Canvas update
            if (Running >= 1 && canvas.Focus == this) { canvas.UpdatePixels(Delta); }
            if (Running == 1) { Running = 0; } // If one frame then pause afterwards
            canvas.UpdateGui(Delta);

            // Final
            previousMouse = mouse;

            // Base
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Start
            spriteBatch.Begin(SpriteSortMode.Deferred);
            GraphicsDevice.Clear(Color.Magenta);

            // Menu backgroumd
            spriteBatch.FillRectangle(new(0, Consts.Screen.Window[1] - (Consts.Gui.MenuSize * Consts.Screen.PixelSize) - 6, Consts.Screen.Window[0], Consts.Gui.MenuSize * Consts.Screen.PixelSize + 6), Color.Gray);

            // Canvas
            canvas.Draw();

            // Outline
            spriteBatch.DrawRectangle(new(0, 0, Consts.Screen.Window[0], Consts.Screen.Window[1]), Registry.Materials.Colors[Registry.Materials.Id(Selection)], 2);

            // Info
            if (canvas.Delta != 0)
            {
                string info = $"Delta: {Math.Round(Delta * 1000, 1)}\nFPS: {(int)(1 / Delta)}\nSpeed: {(Running >= 1 && canvas.Focus == this ? Consts.Game.Speed : 0)}x\nView Mode: {ColorModes[canvas.ViewMode]}\nReplace: {Replace}\nSize: {CursorSize}";
                spriteBatch.DrawString(Font, info, new Vector2(20, 20), canvas.ViewMode != 2 ? Color.Black: Color.White);
            }

            // Feed
            string[] feed = Logger.logged.ToArray();
            spriteBatch.DrawString(SmallFont, string.Join("\n", feed.TakeLast(Consts.Visual.FeedLength)), new Vector2(Consts.Screen.Window[0] - 220, 5), Color.Black);

            // Square
            foreach (Pixel pixel in canvas.Pixels)
            {
                Color ghost = Selection == "Air" || cursor == "Erase" ? new Color(105, 0, 0, 100) : (Replace ? new Color(35, 0, 0, 100) : new Color(75, 75, 75, 100));
                if (Math.Abs(pixel.Coords.X - coord.X) < CursorSize && Math.Abs(pixel.Coords.Y - coord.Y) < CursorSize)
                {
                    spriteBatch.FillRectangle(new(pixel.Snapped.X, pixel.Snapped.Y, Consts.Game.PixelSize, Consts.Game.PixelSize), ghost);
                }
            }

            // Cursor
            if (cursor == "Erase" || Selection == "Air")
            {
                DrawX(spriteBatch, location, [8, 8], Color.Red, 2);
            }
            else if (cursor == "Normal")
            {
                DrawX(spriteBatch, location, [8, 8], Color.Black, 2);
            }
            else if (cursor == "Draw")
            {
                DrawX(spriteBatch, location, [8, 8], Color.Black, 3);
                DrawX(spriteBatch, location, [8, 8], Registry.Materials.Colors[Registry.Materials.Id(Selection)], 2);
            }
            else
            {
                DrawPlus(spriteBatch, location, [12, 12], Color.Green, 3);
            }

            // Drawing
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            // Log closing
            Logger.Log("Window closed");
            base.OnExiting(sender, args);
        }
        public bool KeyPress(Keys key)
        {
            return keys.Contains(key) && !previous.Contains(key);
        }
        // Static methods
        public static bool Inside(Xna.Vector2 point, int[] dimensions)
        {
            return point.X >= 0 && point.X < dimensions[0] && point.Y >= 0 && point.Y < dimensions[1];
        }
        public static bool Inside(Xna.Point point, int[] dimensions) { return Inside(point.ToVector2(), dimensions); }
        public static void DrawX(SpriteBatch batch, Xna.Vector2 location, int[] dimensions, Color color, int thickness = 2)
        {
            //  "\" line
            batch.DrawLine(new(location.X - dimensions[0] / 2, location.Y - dimensions[1] / 2), new(location.X + dimensions[0] / 2, location.Y + dimensions[1] / 2),
                           color, thickness);
            //  "/" line
            batch.DrawLine(new(location.X + dimensions[0] / 2, location.Y - dimensions[1] / 2), new(location.X - dimensions[0] / 2, location.Y + dimensions[1] / 2),
                           color, thickness);
        }
        public static void DrawPlus(SpriteBatch batch, Xna.Vector2 location, int[] dimensions, Color color, int thickness = 1)
        {
            //  "-" line
            batch.DrawLine(new(location.X - dimensions[0] / 2, location.Y), new(location.X + dimensions[0] / 2, location.Y),
                           color, thickness);
            //  "|" line
            batch.DrawLine(new(location.X, location.Y - dimensions[1] / 2), new(location.X, location.Y + dimensions[1] / 2),
                           color, thickness);
        }

        // Other static methods
        public static void EraseMode(Window window) { window.Selection = "Air"; }
        public static void TogglePlay(Window window) { window.Running = window.Running <= 1 ? 2 : 0; }
        public static void ToggleReplace(Window window) { window.Replace = !window.Replace; }
        public static void RunFrame(Window window) { window.Running = 1; }
        public static void CycleSpeed(Window window)
        {
            window.Running = 2;
            Consts.Game.Speed = (int)(Consts.Game.Speed + 1f) % 4;
            if (Consts.Game.Speed == 0) { Consts.Game.Speed = .5f; }
        }
    }
}
