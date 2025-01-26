using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Oil : Fueling
    {
        // Constructor
        public Oil(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Fuel = 15;
            Conductivity = .15f;
            Density = .85f;
            State = 3;
            Strength = 400;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Oil));
        }
    }
}
