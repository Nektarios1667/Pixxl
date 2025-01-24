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
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Pixxl.Gui
{
    public class Popup : Widget
    {
        public SpriteBatch Batch { get; private set; }
        public Xna.Vector2 Dimensions { get; private set; }
        public Rectangle Rect
        {
            get { return new((int)Location.X, (int)Location.Y, (int)Dimensions.X, (int)Dimensions.Y); }
        }
        public Xna.Color Color { get; private set; }
        public Delegate? Function { get; private set; }
        public int Border { get; private set; }
        public Color BorderColor { get; private set; }
        public Color BarColor { get; private set; }
        public int BarSize { get; private set; }
        public Xna.Vector2 BarDimensions { get; private set; }
        public Xna.Vector2 LastBarPosition { get; private set; }
        public MouseState Previous { get; private set; }
        public Rectangle BarRect
        {
            get { return new((int)Location.X, (int)Location.Y, (int)BarDimensions.X, (int)BarSize); }
        }
        private bool Dragging { get; set; }
        public List<Widget> Widgets { get; set; }
        public string Title { get; set; }
        public SpriteFont TitleFont { get; set; }
        public Color TitleColor { get; set; }
        // Centering
        public Popup(SpriteBatch batch, Xna.Vector2 location, Xna.Vector2 dimensions, Color color, string title, SpriteFont titleFont, Color? titleColor = null, Color? barColor = null, int barSize = 25, int border = 3, Color? borderColor = null)
        {
            Batch = batch;
            Location = location;
            Dimensions = dimensions;
            Color = color;
            Border = border;
            BorderColor = (borderColor == null ? Color.Black : (Color)borderColor);
            BarColor = (barColor == null ? Color.DarkGray : (Color)barColor);
            BarSize = barSize;
            BarDimensions = new(dimensions.X, barSize);
            Visible = true;
            LastBarPosition = new(-1, -1);
            Previous = new();
            TitleColor = titleColor == null ? Color.Black : (Color)titleColor;
            Button closeButton = new(batch, new(location.X + dimensions.X - 50, location.Y), new(50, 25), Color.White, Color.Red, new(255, 75, 75), Close, args: [this]);
            TextBox titleBox = new(Batch, new(location.X + 10, location.Y + 4), TitleColor, title, titleFont);
            Widgets = [closeButton, titleBox];
            TitleFont = titleFont;
            Title = title;

        }
        public override void Update(MouseState mouseState)
        {
            // Hovering
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Clicks
                if (PointRectCollide(Location, BarDimensions, mouseState.Position) || Dragging)
                {
                    // Dragging
                    if (Previous.LeftButton == ButtonState.Pressed && LastBarPosition.X != -1) {
                        Xna.Vector2 delta = mouseState.Position.ToVector2() - LastBarPosition;
                        foreach (Widget widget in Widgets) { widget.Location += delta; }
                        Location += delta;
                        Dragging = true;
                    }
                    LastBarPosition = mouseState.Position.ToVector2();
                }
            } else { Dragging = false; }

            // Widgets update
            foreach (Widget widget in Widgets) { widget.Update(mouseState); }

            // Previous
            Previous = mouseState;
        }
        public override void Draw()
        {
            // Not drawing
            if (!Visible) { return; }

            // Background
            Batch.FillRectangle(Rect, Color);
            // Bar
            Batch.FillRectangle(BarRect, BarColor);
            // Widgets update
            foreach (Widget widget in Widgets) { widget.Draw(); }
            // Outline
            Batch.DrawRectangle(Rect, BorderColor, Border);
        }

        public void AddWidgets(params Widget[] widgets) { foreach (Widget widget in widgets) { Widgets.Add(widget); } }

        // static
        public static void Close(Popup popup) { popup.Visible = false; }
    }
}
