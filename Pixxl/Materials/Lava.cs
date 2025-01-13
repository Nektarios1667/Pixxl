using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Lava : Pixel
    {
        // Constructor
        public Lava(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 6000f;
            Conductivity = .05f;
            Density = 2.4f;
            State = 3;
            Strength = 1000;
            Melting = new Transformation(999999, typeof(Lava));
            Solidifying = new Transformation(1800, typeof(Concrete));
        }
        public override void Update()
        {
            base.Update();

            Logger.Log(Density.ToString(), Temperature.ToString());
        }
    }
}
