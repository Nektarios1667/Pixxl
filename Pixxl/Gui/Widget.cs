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
    public abstract class Widget
    {
        public Xna.Vector2 Location { get; set; }
        public bool Visible { get; set; }
        public Widget() { Visible = true; }
        public abstract void Update(MouseState mouseState);
        public abstract void Draw();

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
