using System.Collections.Generic;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class GasolineVapor : Fuel
    {
        // Constructor
        public GasolineVapor(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Ashes = false;
            Lifetime = .1f;
            Temperature = 180;
            Conductivity = .04f;
            Density = 0.75f;
            State = 3;
            Strength = 130;
            Melting = new Transformation(9500, typeof(Plasma));
            Solidifying = new Transformation(80, typeof(Gasoline));
        }
        public override void Update()
        {
            base.Update();

            // Fire travel
            foreach (Pixel? neighbor in Neighbors)
            {
                // Exposed to burning item
                if (neighbor != null && neighbor is IBurnable burnable && burnable.Lit) { Lit = true; }
            }
        }
    }
}
