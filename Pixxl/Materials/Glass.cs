using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Glass : Pixel
    {
        // Constructor
        public Glass(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .9f;
            Density = .918f;
            State = 0;
            Strength = 80;
            Melting = new Transformation(999999, typeof(Glass));
            Solidifying = new Transformation(-999999, typeof(Glass));
            Gravity = false;
        }
    }
}
