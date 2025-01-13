using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;

namespace Pixxl.Materials
{
    public class Coal : Fueling
    {
        // Constructor
        public Coal(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Fuel = 10f;
            Burned = 0f;
            Conductivity = .3f;
            Density = 1.5f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(999999, typeof(Coal));
            Solidifying = new Transformation(-999999, typeof(Coal));
        }
    }
}
