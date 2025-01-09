using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Sand : Pixel
    {
        // Constructor
        public Sand(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .24f;
            Density = 1.5f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(3100, typeof(Glass));
            Solidifying = new Transformation(-999999, typeof(Sand));
        }
    }
}
