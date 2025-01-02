using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Water : Pixel
    {
        // Constructor
        public Water(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Density = 1f;
            State = 3;
            Strength = 999999;
            Melting = 212;
            Color = ColorSchemes.Water();
        }
    }
}
