using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;

namespace Pixxl.Materials
{
    public class Coal : Fuel
    {
        // Constructor
        public Coal(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 10f;
            Conductivity = .3f;
            Density = 1.8f;
            Strength = 50;
            Melting = new Transformation(999999, typeof(Coal));
            Solidifying = new Transformation(-999999, typeof(Coal));
        }
    }
}
