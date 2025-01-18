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
        public bool Superheated { get; set; }
        public bool Internal { get; set; }
        public bool Ashes { get; set; }
        // Constructor
        public Fueling(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Ashes = true;
            Internal = false;
            Superheated = false;
            Lit = false;
            Fuel = 10f;
            Burned = 0f;
            Conductivity = .3f;
            Density = 1.5f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(999999, typeof(Coal));
            Solidifying = new Transformation(-999999, typeof(Coal));
        }

        public override void Update()
        {
            base.Update();
            // Burned out
            if (Burned >= Fuel) {
                Pixel creation = State <= 2 && Ashes && Canvas.Rand.Next(0, 4) == 0 ? new Ash(Location, Canvas) : Superheated ? new BlueFire(Location, Canvas) : new Fire(Location, Canvas);
                Canvas.Pixels[Flat(Coords)] = creation;
                return;
            }

            // Lit
            if (Lit)
            {
                Burned += Canvas.Delta;
                // Burn
                Xna.Vector2 spawn = new(Location.X, Location.Y - Constants.Screen.PixelSize);
                if (spawn.Y < 0) { return; }
                int idx = Flat(Coord(spawn));

                if (Canvas.Pixels[idx].Type == "Air")
                {
                    Canvas.Pixels[idx] = Ashes && Canvas.Rand.Next(0, 40) == 0 ? new Smoke(spawn, Canvas) : Superheated ? new BlueFire(spawn, Canvas) : new Fire(spawn, Canvas);
                }
                // Snuffed
                else if (Canvas.Pixels[idx].State != 4  && Canvas.Pixels[idx].Type != "Smoke" && !Internal)
                {
                    Lit = false;
                }
            }
        }
    }
}
