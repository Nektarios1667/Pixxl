using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Xna = Microsoft.Xna.Framework;
using Pixxl.Materials;
using Pixxl.Tools;
using Consts = Pixxl.Constants;
using System.Data;

namespace Pixxl
{
    public class Window : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Canvas canvas { get; private set; }
        public Xna.Vector2 snapped { get; private set; }
        private Keys[] previous { get; set; } = [];
        public SpriteFont Font { get; set; }
        public string Selection { get; set; }
        public 

        Window()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Constants.Screen.Window[0],
                PreferredBackBufferHeight = Constants.Screen.Window[1]
            };
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Selection = "Concrete";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Arial");
            
            // Load canvas at the end
            canvas = new(this, _graphics.GraphicsDevice, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            // States
            KeyboardState keyState = Keyboard.GetState();
            Keys[] keys = keyState.GetPressedKeys();
            MouseState mouse = Mouse.GetState();
            Xna.Vector2 location = new(mouse.Position.X, mouse.Position.Y);
            snapped = new ((int)Math.Floor((float)location.X / Consts.Screen.PixelSize), (int)Math.Floor((float)location.Y / Consts.Screen.PixelSize));

            // Exit
            if (keys.Contains(Keys.Escape))
                Exit();

            // Drawing
            if (mouse.LeftButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Consts.Screen.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Consts.Screen.Grid[1] - 1)
            {
                if (canvas.Pixels[Pixel.Flat(snapped.Y, snapped.X)].GetType().Name == "Air")
                {
                    canvas.Pixels[Pixel.Flat(snapped.Y, snapped.X)] = Canvas.New(canvas, Selection, location);
                }
            }
            else if (mouse.MiddleButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Consts.Screen.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Consts.Screen.Grid[1] - 1)
            {
                canvas.Pixels[Pixel.Flat(snapped.Y, snapped.X)] = new Air(location, canvas);
            }

            // Keyboard
            if (keys.Contains(Keys.OemTilde) && !previous.Contains(Keys.OemTilde))
            {
                canvas.ColorMode = (canvas.ColorMode + 1) % 3;
            }
            previous = keys.ToArray();

            // Canvas update
            canvas.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000f, mouse);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            // Canvas
            canvas.Draw();

            // Text
            if (canvas.Delta != 0)
            {
                _spriteBatch.DrawString(Font, $"Delta: {Math.Round(canvas.Delta * 1000, 1)}\nFPS: {Math.Round(1 / canvas.Delta, 0)}", new Vector2(20, 20), Color.Black);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
