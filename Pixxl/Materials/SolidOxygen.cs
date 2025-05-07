using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Numerics;

namespace Pixxl.Materials
{
    public class SolidOxygen : Pixel
    {
        // Constructor
        public SolidOxygen(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -350;
            Conductivity = .5f;
            Density = 1.42f;
            State = 0;
            Strength = 60;
            Melting = new Transformation(-218, typeof(LiquidOxygen));
            Solidifying = new Transformation(-999999, typeof(SolidOxygen));
            Gravity = false;
        }
    }
}
