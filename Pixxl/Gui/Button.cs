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
        public bool Last { get; private set; }
        public bool Visible { get; set; }
        // Centering
        private Xna.Vector2 offset { get; set; }
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
            Last = false;
            Visible = true;

            Xna.Vector2 textDim = Font.MeasureString(Text);
            Xna.Vector2 inside = new(Dimensions.X - Border * 2, Dimensions.Y - Border * 2);
            offset = Xna.Vector2.Floor((inside - textDim) / 2);
        }
        public void Update(MouseState mouseState)
        {
            // Hovering
            bool pressed = false;
            if (PointRectCollide(Location, Dimensions, mouseState.Position))
            {
                // Clicking
                pressed = mouseState.LeftButton == ButtonState.Pressed;
                if (pressed && !Last)
                {
                    State = 2;
                    Function?.DynamicInvoke(Args);
                }
                else { State = 1; }
            } else { State = 0; }
            Last = pressed;
        }
        public void Draw()
        {
            // Not drawing
            if (!Visible) { return; }

            // Background
            Batch.FillRectangle(Rect, State == 0 ? Color : Highlight);
            // Outline
            Batch.DrawRectangle(Rect, State == 0 ? BorderColor : State == 1 ? Tools.Functions.Lighten(BorderColor, .2f) : Tools.Functions.Lighten(BorderColor, .4f), Border);

            // Text
            if (Font != null)
            {
                Batch.DrawString(Font, Text, new(Location.X + Border + offset.X, Location.Y + Border + offset.Y), Foreground);
            } else
            {
                Logger.Log($"Skipping drawing text '{Text}' because of uninitialized font");
            }
        }

        // Static methods
        public static bool PointRectCollide(Xna.Vector2 loc, Xna.Vector2 dim, Xna.Vector2 point)
        {
            return (point.X >= loc.X && point.X <= loc.X + dim.X) && (point.Y >= loc.Y && point.Y <= loc.Y + dim.Y);
        }
        public static bool PointRectCollide(Xna.Vector2 loc, Xna.Vector2 dim, Xna.Point point)
        {
            return PointRectCollide(loc, dim, point.ToVector2());
        }
    }
}
