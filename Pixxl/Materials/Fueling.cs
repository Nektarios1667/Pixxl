using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public abstract class Fueling : Pixel
    {
        public float Fuel { get; set; }
        public float Burned { get; set; }
        public bool Lit { get; set; }
        // Constructor
        public Fueling(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lit = false;
            Fuel = 10f;
            Burned = 0f;
            Conductivity = .3f;
            Density = 1.5f;
            State = 3;
            Strength = 50;
            Melting = new Transformation(999999, typeof(Coal));
            Solidifying = new Transformation(-999999, typeof(Coal));
        }

        public override void Update()
        {
            base.Update();
            // Burned out
            if (Burned >= Fuel) { Canvas.Pixels[Flat(Coords)] = new Fire(Location, Canvas); return; }

            // Lit
            if (Lit)
            {
                Burned += Canvas.Delta;
                // Burn
                Xna.Vector2 spawn = new(Location.X, Location.Y - Constants.Screen.PixelSize);
                int idx = Flat(Coord(spawn));
                if (Canvas.Pixels[idx].Type == "Air")
                {
                    Canvas.Pixels[idx] = new Fire(spawn, Canvas);
                }
            }
        }
    }
}
