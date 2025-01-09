using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Helium : Pixel
    {
        // Constructor
        public Helium(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .142f;
            Density = .00018f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Helium));
        }
    }
}
