using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Xna = Microsoft.Xna.Framework;
using Pixxl.Materials;
using MonoGame.Extended;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pixxl.Gui
{
    public class Button
    {
        public SpriteBatch Batch { get; private set; }
        public Xna.Vector2 Location { get; private set; }
        public Xna.Vector2 Dimensions { get; private set; }
        public string Text { get; private set; }
        public Rectangle Rect
        {
            get { return new((int)Location.X, (int)Location.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }
        public Xna.Color Color { get; private set; }
        public Xna.Color Highlight { get; private set; }
        public SpriteFont Font { get; private set; }
        public Color Foreground { get; private set; }
        public Delegate? Function { get; private set; }
        public int Border { get; private set; }
        public Color BorderColor { get; private set; }
        public object?[]? Args { get; private set; }
        public int State { get; private set; }
        public Button(SpriteBatch batch, Xna.Vector2 location, Xna.Vector2 dimensions, string text, SpriteFont font, Color foreground, Xna.Color color, Xna.Color highlight, Delegate? function, object?[]? args, int border = 3, Color borderColor = default)
        {
            Batch = batch;
            Location = location;
            Dimensions = dimensions;
            Text = text;
            Font = font;
            Foreground = foreground;
            Color = color;
            Highlight = highlight;
            Function = function;
            Border = border;
            BorderColor = (borderColor == default ? Color.Black : borderColor);
            Args = args;
            State = 0;
        }
        public void Update(MouseState mouseState)
        {
            State = 0;
            // Hovering
            if (PointRectCollide(Location, Dimensions, mouseState.Position))
            {
                // Clicking
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    State = 2;
                    Function?.DynamicInvoke(Args);
                }
                else { State = 1; }
            }
        }
        public void Draw()
        {
            // Background
            Batch.FillRectangle(Rect, State == 0 ? Color : Highlight);
            // Outline
            Batch.DrawRectangle(Rect, State == 0 ? BorderColor : State == 1 ? new(35, 35, 35) : new(65, 65, 65), Border);
            // Text
            if (Font != null)
            {
                Batch.DrawString(Font, Text, new(Location.X + Border + 3, Location.Y + Border + 3), Foreground);
            } else
            {
                Console.WriteLine($"Skipping drawing text '{Text}' because of uninitialized font.");
            }
        }

        // Static methods
        public bool PointRectCollide(Xna.Vector2 loc, Xna.Vector2 dim, Xna.Vector2 point)
        {
            return (point.X >= loc.X && point.X <= loc.X + dim.X) && (point.Y >= loc.Y && point.Y <= loc.Y + dim.Y);
        }
        public bool PointRectCollide(Xna.Vector2 loc, Xna.Vector2 dim, Xna.Point point)
        {
            return (point.X >= loc.X && point.X <= loc.X + dim.X) && (point.Y >= loc.Y && point.Y <= loc.Y + dim.Y);
        }
    }
}
