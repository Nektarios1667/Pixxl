using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Wood : Fueling
    {
        // Constructor
        public Wood(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = .6f;
            State = 0;
            Fuel = 4;
            Strength = 100;
            Melting = new Transformation(999999, typeof(Wood));
            Solidifying = new Transformation(-999999, typeof(Wood));
            Gravity = false;
            Internal = true;
        }
    }
}
