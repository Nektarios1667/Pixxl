using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Sodium : Pixel
    {
        // Constructor
        private bool Reacting { get; set; }
        private float Burned { get; set; }
        public Sodium(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .1f;
            Density = .9f;
            State = 2;
            Strength = 150;
            Melting = new Transformation(999999, typeof(Sodium));
            Solidifying = new Transformation(-999999, typeof(Sodium));
        }

        public override void Update()
        {
            base.Update();

            // Burning
            if (Reacting) { Burned += Canvas.Delta; }
            if (Burned >= 4) { Skip = true; Canvas.Pixels[Index] = new Fire(Location, Canvas); }

            // Water reaction
            bool touchingWater = false;
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor == null) { continue; } // null neighbor
                if (!Reacting && neighbor.Type == "Water") { Reacting = true; touchingWater = true; }
                else if (Reacting && neighbor.Type == "Air") { neighbor.Skip = true; Canvas.Pixels[neighbor.Index] = new Fire(neighbor.Location, Canvas); }
            }

            // Not touching water anymore
            if (!touchingWater) { Reacting = false; }
        }
    }
}
