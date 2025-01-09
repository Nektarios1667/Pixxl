using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Chiller : Pixel
    {
        // Constructor
        public Chiller(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = -100f;
            Conductivity = 60f;
            Density = 10f;
            State = 0;
            Gravity = false;
            Strength = 999999;
            Melting = new Transformation(999999, typeof(Chiller));
            Solidifying = new Transformation(-999999, typeof(Chiller));
        }
        public override void Update()
        {
            base.Update();

            Temperature = -100f;
        }

    }
}
