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

namespace Pixxl
{
    public class Window : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public Canvas canvas;
        public Xna.Vector2 snapped;
        public SpriteFont Font;
        public SpriteFont SmallFont;
        public string Selection;
        public Pixel? hovering;
        public Xna.Vector2 location = new(0, 0);

        private Keys[] previous = [];
        private Keys[] keys = [];
        private MouseState mouse;

        public Window()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Consts.Screen.Window[0],
                PreferredBackBufferHeight = Consts.Screen.Window[1]
            };
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            Content.RootDirectory = "Content";
            Selection = "Concrete";

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
        }

        protected override void Update(GameTime gameTime)
        {
            // States
            keys = Keyboard.GetState().GetPressedKeys();
            mouse = Mouse.GetState();
            location = mouse.Position.ToVector2();
            snapped = new((int)Math.Floor((float)location.X / Consts.Screen.PixelSize), (int)Math.Floor((float)location.Y / Consts.Screen.PixelSize));

            // Hovering
            int idx = Pixel.Flat(Pixel.ConvertToCoord(location, 'l'));
            hovering = idx < canvas.Pixels.Length && idx > 0 ? canvas.Pixels[idx] : null;

            // Exit
            if (keys.Contains(Keys.Escape))
                Exit();

            // Drawing
            if (mouse.LeftButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Consts.Screen.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Consts.Screen.Grid[1] - 1)
            {
                if (canvas.Pixels[Pixel.Flat(snapped)].GetType().Name == "Air")
                {
                    canvas.Pixels[Pixel.Flat(snapped)] = Canvas.New(canvas, Selection, location);
                }
            }
            else if (mouse.MiddleButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Consts.Screen.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Consts.Screen.Grid[1] - 1)
            {
                canvas.Pixels[Pixel.Flat(snapped)] = new Air(location, canvas);
            }

            // Keyboard
            if (keys.Contains(Keys.OemTilde) && !previous.Contains(Keys.OemTilde))
            {
                canvas.ColorMode = (canvas.ColorMode + 1) % 4;
            }
            previous = keys.ToArray();

            // Canvas update
            canvas.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000f, mouse);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Canvas
            canvas.Draw();

            // Text
            if (canvas.Delta != 0)
            {
                spriteBatch.DrawString(Font, $"Delta: {Math.Round(canvas.Delta * 1000, 1)}\nFPS: {Math.Round(1 / canvas.Delta, 0)}", new Vector2(20, 20), Color.Black);
            }

            // Feed
            string[] feed = Logger.logged.ToArray();
            spriteBatch.DrawString(SmallFont, string.Join("\n", feed.TakeLast(Consts.Visual.FeedLength)), new Vector2(Constants.Screen.Window[0] - 220, 5), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
