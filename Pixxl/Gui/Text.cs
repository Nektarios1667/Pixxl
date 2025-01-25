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
    public class TextBox : Widget
    {
        public SpriteBatch Batch { get; private set; }
        public string Text { get; private set; }
        public Xna.Color Color { get; private set; }
        public SpriteFont? Font { get; private set; }
        // Centering
        public TextBox(SpriteBatch batch, Xna.Vector2 location, Color color, string text, SpriteFont font)
        {
            Batch = batch;
            Location = location;
            Text = text;
            Font = font;
            Color = color;
        }
        public override void Update(Window _) {}
        public override void Draw()
        {
            // Not drawing
            if (!Visible) { return; }

            // Text
            Batch.DrawString(Font, Text, Location, Color);
        }
    }
}
