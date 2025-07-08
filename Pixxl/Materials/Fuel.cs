using System;
using System.Linq;
using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    interface IIgnitable
    {
        void Ignite();
        void Snuff();
    }
    interface IBurnable : IIgnitable
    {
        float Lifetime { get; set; }
        float Burned { get; set; }
        bool Lit { get; set; }
        bool Superheated { get; set; }
        bool Internal { get; set; }
        bool Ashes { get; set; }
    }

    public abstract class Fuel : Pixel, IBurnable
    {
        public float Lifetime { get; set; }
        public float Burned { get; set; }
        public bool Lit { get; set; }
        public bool Superheated { get; set; }
        public bool Internal { get; set; }
        public bool Ashes { get; set; }
        private readonly string[] nonSnuffable = { "Smoke", "Fire", "BlueFire" };
        // Constructor
        public Fuel(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Ashes = true;
            Internal = false;
            Superheated = false;
            Lit = false;
            Lifetime = 10f;
            Burned = 0f;
            Conductivity = .3f;
            Density = 1.5f;
            State = 2;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Coal));
            Solidifying = new Transformation(Int32.MinValue, typeof(Coal));
        }

        public override void Update()
        {
            base.Update();

            // Burned out
            if (Burned >= Lifetime)
            {
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
                    if (neighbor.Type == "Air")
                    {
                        neighbor.Skip = true;
                        AirPool.Return((Air)neighbor);
                        Canvas.Pixels[neighbor.GetIndex()] = Ashes && Canvas.Rand.Next(0, 40) == 0 ? new Smoke(neighbor.Location, Canvas)
                            : Superheated ? new BlueFire(neighbor.Location, Canvas)
                            : new Fire(neighbor.Location, Canvas);
                    }
                    else if (n == 0 && neighbor.State != 4 && !nonSnuffable.Contains(neighbor.Type) && !Internal) // Snuffed
                    {
                        Snuff();
                        break;
                    }
                    n++;
                }
            }
        }

        public void Ignite()
        {
            Lit = true;
        }

        public void Snuff()
        {
            Lit = false;
        }
    }
}
