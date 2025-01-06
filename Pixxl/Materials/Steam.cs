using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Steam : Pixel
    {
        // Constructor
        public Steam(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 250f;
            Conductivity = .05f;
            Density = .0004f;
            State = 3;
            Strength = 100;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(180, typeof(Water));
            Color = ColorSchemes.Steam();
        }
    }
}
