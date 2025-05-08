using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Consts = Pixxl.Constants;

namespace Pixxl.Materials
{
    public class FlareSmoke : Pixel
    {
        private float Life { get; set; }
        // Constructor
        public FlareSmoke(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 800f;
            Conductivity = .02f;
            Density = .00042f;
            State = 3;
            Strength = 90;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(400, typeof(Air));
            Life = 0;
        }
        public override void Update()
        {
            base.Update();

            // Dissipate
            Life += Canvas.Delta;
            if (Life > 4)
            {
                Canvas.Pixels[Index] = new Air(Location, Canvas);
                Canvas.Pixels[Index].Skip = true;
                return;
            }
        }
    }
}
