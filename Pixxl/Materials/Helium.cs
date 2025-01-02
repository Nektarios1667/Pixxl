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
            Density = .00018f;
            State = 3;
            Strength = 999999;
            Melting = 9500;
            Color = ColorSchemes.Helium();
        }
    }
}
