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

        public Xna.Vector2 snapped { get; private set; }

        public Window()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 900
            };
            IsFixedTimeStep = false;
            IsMouseVisible = true;
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
            if (mouse.LeftButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] && snapped.Y >= 0 && snapped.Y <= Const.Grid[1])
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Sand(location, canvas);
                }
            } else if (mouse.RightButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] && snapped.Y >= 0 && snapped.Y <= Const.Grid[1])
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Helium(location, canvas);
                }
            }
            else if (mouse.MiddleButton == ButtonState.Pressed && snapped.X >= 0 && snapped.X <= Const.Grid[0] && snapped.Y >= 0 && snapped.Y <= Const.Grid[1])
            {
                if (canvas.Pixels[(int)snapped.Y][(int)snapped.X].GetType().Name == "Air")
                {
                    canvas.Pixels[(int)snapped.Y][(int)snapped.X] = new Concrete(location, canvas);
                }
            }

            // Canvas update
            canvas.Update((float)gameTime.ElapsedGameTime.Milliseconds / 1000f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Canvas
            canvas.Draw();

            base.Draw(gameTime);
        }
    }
}
