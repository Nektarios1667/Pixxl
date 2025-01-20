using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Acid : Pixel
    {
        // Constructor
        public Acid(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .02f;
            Density = 1.2f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(8200, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Acid));
        }
        public override void Update()
        {
            base.Update();

            // Burning
            Pixel? down = Neighbors[4];
            if (down != null && down.Strength < 500 && Canvas.Rand.Next(0, down.Strength) == 0)
            {
                Canvas.Pixels[down.Index].Skip = true;
                Canvas.Pixels[down.Index] = new Air(down.Location, Canvas);
            }
        }
    }
}
