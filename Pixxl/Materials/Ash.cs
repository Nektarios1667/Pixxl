using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Ash : Pixel
    {
        // Constructor
        public Ash(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 200f;
            Conductivity = .14f;
            Density = 1.3f;
            State = 2;
            Strength = 30;
            Melting = new Transformation(999999, typeof(Ash));
            Solidifying = new Transformation(-999999, typeof(Ash));
        }
    }
}
