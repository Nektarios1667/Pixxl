using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Fuse : Fueling
    {
        // Constructor
        float Time { get; set; }
        public Fuse(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Ashes = false;
            Time = 0f;
            Internal = true;
            Fuel = .5f;
            Conductivity = .05f;
            Density = .8f;
            State = 0;
            Strength = 50;
            Melting = new Transformation(1000, typeof(BlueFire));
            Solidifying = new Transformation(-999999, typeof(Fuse));
            Gravity = false;
        }

        // Update
        public override void Update()
        {
            base.Update();

            // Fuse travel
            bool exposed = false;
            foreach (Pixel neighbor in Neighbors)
            {
                // Exposed to fuse
                if (neighbor.Type == "Fuse" && ((Fuse)neighbor).Lit)
                {
                    // Exposed for long enough to ignite
                    exposed = true;
                    Time += Canvas.Delta;
                    if (Time >= Fuel) { Lit = true; }
                }
                
                // Surrounding fire
                if (Lit && neighbor.Type == "Air")
                {
                    Canvas.Pixels[Flat(neighbor.Coords)].Ignore = true;
                    Canvas.Pixels[Flat(neighbor.Coords)] = new Fire(neighbor.Location, Canvas);
                }
            }

            // Reset if not exposed
            if (!exposed) { Time = 0; }
        }
    }
}
