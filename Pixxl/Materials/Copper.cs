using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Copper : Pixel
    {
        // Constructor
        public Copper(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 60f;
            Density = 9f;
            State = 0;
            Strength = 200;
            Melting = new Transformation(1900, typeof(Lava));
            Solidifying = new Transformation(-999999, typeof(Copper));
            Gravity = false;
        }
    }
}
