using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class CoolantVapor : Pixel
    {
        // Constructor
        public CoolantVapor(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 800;
            Conductivity = 4012f;
            Density = .0003f;
            State = 3;
            Strength = 600;
            Melting = new Transformation(14000, typeof(Plasma));
            Solidifying = new Transformation(600, typeof(Coolant));
        }
    }
}
