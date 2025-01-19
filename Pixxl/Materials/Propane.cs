using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Propane : Fueling
    {
        // Constructor
        public Propane(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Fuel = .2f;
            Conductivity = .09f;
            Density = .00018f;
            State = 3;
            Strength = 999999;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(-999999, typeof(Program));
        }

        public override void Update()
        {
            base.Update();

            // Fire travel
            foreach (Pixel? neighbor in Neighbors)
            {
                // Exposed to burning item
                if (neighbor != null && neighbor.GetType().BaseType.Name == "Fueling" && ((Fueling)neighbor).Lit)
                {
                    Lit = true;
                }
            }
        }
    }
}
