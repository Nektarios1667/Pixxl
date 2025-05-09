using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Gui
{
    public class Button : Widget
    {
        public SpriteBatch Batch { get; private set; }
        public Xna.Vector2 Dimensions { get; private set; }
        public string Text { get; private set; }
        public Rectangle Rect
        {
            get { return new((int)Location.X, (int)Location.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }
        public Xna.Color Color { get; private set; }
        public Xna.Color Highlight { get; private set; }
        public SpriteFont? Font { get; private set; }
        public Color Foreground { get; private set; }
        public Delegate? Function { get; private set; }
        public int Border { get; private set; }
        public Color BorderColor { get; private set; }
        public object?[]? Args { get; private set; }
        public int State { get; private set; }
        public bool Last { get; private set; }
        // Centering
        private Xna.Vector2 offset { get; set; }
        public Button(SpriteBatch batch, Xna.Vector2 location, Xna.Vector2 dimensions, Color foreground, Xna.Color color, Xna.Color highlight, Delegate? function, string text = "", SpriteFont? font = null, object?[]? args = null, int border = 3, Color borderColor = default)
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

            Xna.Vector2 textDim = Font != null ? Font.MeasureString(Text) : new(0, 0);
            Xna.Vector2 inside = new(Dimensions.X - Border * 2, Dimensions.Y - Border * 2);
            offset = Xna.Vector2.Floor((inside - textDim) / 2);
        }
        public override void Update(Window window)
        {
            // Hidden
            if (!Visible) { return; }

            // Hovering
            MouseState mouseState = window.mouse;
            bool pressed = mouseState.LeftButton == ButtonState.Pressed;
            if (PointRectCollide(Location, Dimensions, mouseState.Position))
            {
                // Clicking
                if (pressed && !Last)
                {
                    State = 2;
                    Function?.DynamicInvoke(Args);
                }
                else { State = 1; }
            }
            else { State = 0; }
            Last = pressed;
        }
        public override void Draw()
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
            }
            else if (Text != "")
            {
                Logger.Log($"Skipping drawing text '{Text}' because of uninitialized font");
            }
        }
    }
}
