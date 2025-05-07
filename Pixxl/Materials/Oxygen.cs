using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Numerics;

namespace Pixxl.Materials
{
    public class Oxygen : Pixel
    {
        // Constructor
        public Oxygen(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .015f;
            Density = .0013f;
            State = 3;
            Strength = 60;
            Melting = new Transformation(9500, typeof(Plasma));
            Solidifying = new Transformation(-279, typeof(LiquidOxygen));
        }
    }
}
