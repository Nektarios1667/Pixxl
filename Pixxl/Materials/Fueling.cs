using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Linq;

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
        private readonly string[] nonSnuffable = { "Smoke", "Fire", "BlueFire" };
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
                Canvas.Pixels[Index] = creation;
                return;
            }

            // Burn
            if (Lit)
            {
                Burned += Canvas.Delta;
                // Neighbors
                int n = 0;
                foreach (Pixel? neighbor in Neighbors)
                {
                    if (neighbor == null) { n++; continue; }
                    if (neighbor.Type == "Air") {
                        neighbor.Skip = true;
                        Canvas.Pixels[neighbor.GetIndex()] = Ashes && Canvas.Rand.Next(0, 40) == 0 ? new Smoke(neighbor.Location, Canvas)
                            : Superheated ? new BlueFire(neighbor.Location, Canvas)
                            : new Fire(neighbor.Location, Canvas);
                    }
                    else if (n == 0 && neighbor.State != 4 && !nonSnuffable.Contains(neighbor.Type) && !Internal) // Snuffed
                    {
                        Lit = false;
                        break;
                    }
                    n++;
                }
            }
        }
    }
}
