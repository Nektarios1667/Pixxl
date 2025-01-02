using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Plasma : Pixel
    {
        // Constructor
        public Plasma(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 1100f;
            Conductivity = .3f;
            Density = .00018f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(999999, typeof(Plasma));
            Solidifying = new Transformation(9500, typeof(Air));
            Color = ColorSchemes.Plasma();
        }
    }
}
