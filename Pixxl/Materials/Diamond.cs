using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Diamond : Pixel
    {
        // Constructor
        public Diamond(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 2400f;
            Density = 3.51f;
            State = 2;
            Strength = 1000;
            Melting = new Transformation(8500, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Diamond));
        }
    }
}
