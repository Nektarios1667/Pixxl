using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Gravel : Pixel
    {
        // Constructor
        public Gravel(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .08f;
            Density = 1.2f;
            State = 2;
            Strength = 150;
            Melting = new Transformation(2200, typeof(Lava));
            Solidifying = new Transformation(-999999, typeof(Dirt));
        }
    }
}
