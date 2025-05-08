using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Propane : Fuel, IBurnable
    {
        // Constructor
        public Propane(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = .1f;
            Conductivity = .09f;
            Density = .00018f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Program));
            Ashes = false;
            Internal = true;
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
