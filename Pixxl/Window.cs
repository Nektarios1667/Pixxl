using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Xna = Microsoft.Xna.Framework;
using Pixxl.Materials;

namespace Pixxl
{
    public class Window : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Canvas canvas { get; private set; }
        public SpriteFont font { get; private set; }

        public Xna.Vector2 snapped { get; private set; }
        private Keys[] previous { get; set; } = [];

        public Window()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 900
            };
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            canvas = new(_spriteBatch);
            font = Content.Load<SpriteFont>("Arial");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // States
            KeyboardState keyState = Keyboard.GetState();
            Keys[] keys = keyState.GetPressedKeys();
            MouseState mouse = Mouse.GetState();
            Xna.Vector2 location = new(mouse.Position.X, mouse.Position.Y);
            snapped = new ((int)Math.Floor((float)location.X / Const.PixelSize), (int)Math.Floor((float)location.Y / Const.PixelSize));

            // Exit
            if (keys.Contains(Keys.Escape))
                Exit();

            // Drawing
            if (mouse.LeftButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Const.Grid[1] - 1)
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Water(location, canvas);
                }
            } else if (mouse.RightButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Const.Grid[1] - 1)
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Ice(location, canvas);
                }
            }
            else if (mouse.MiddleButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] - 1 && snapped.Y >= 0 && snapped.Y <= Const.Grid[1] - 1)
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Lava(location, canvas);
                }
            }

            // Keyboard
            if (keys.Contains(Keys.OemTilde) && !previous.Contains(Keys.OemTilde))
            {
                canvas.ColorMode = (canvas.ColorMode + 1) % 3;
            }
            previous = keys.ToArray();

            // Canvas update
            canvas.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            // Canvas
            canvas.Draw();

            // Text
            if (canvas.Delta != 0)
            {
                _spriteBatch.DrawString(font, $"Delta: {Math.Round(canvas.Delta * 1000, 1)}\nFPS: {Math.Round(1 / canvas.Delta, 0)}", new Vector2(20, 20), Color.Black);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
