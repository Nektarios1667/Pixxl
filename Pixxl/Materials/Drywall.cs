using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Drywall : Pixel
    {
        // Constructor
        public Drywall(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .1f;
            Density = .3f;
            State = 1;
            Strength = 80;
            Melting = new Transformation(999999, typeof(Drywall));
            Solidifying = new Transformation(-999999, typeof(Drywall));
            Gravity = false;
        }
    }
}
