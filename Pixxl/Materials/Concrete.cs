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
            Density = 2.4f;
            State = 0;
            Strength = 400;
            Melting = 1500;
            Gravity = false;
            Color = ColorSchemes.Concrete();
        }
    }
}
