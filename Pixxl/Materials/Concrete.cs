using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Concrete : Pixel
    {
        // Constructor
        public Concrete(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Density = 2.8f;
            State = 0;
            Strength = 400;
            Melting = new Transformation(2200, typeof(Lava));
            Solidifying = new Transformation(-999999, typeof(Concrete));
            Gravity = false;
            Color = ColorSchemes.Concrete();
        }
    }
}
