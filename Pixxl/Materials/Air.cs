using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Air : Pixel
    {
        // Constructor
        public Air(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .025f;
            Density = .0012f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(9500, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Air));
        }
    }
}
