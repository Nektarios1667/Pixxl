using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Pixxl;

namespace Pixxl.Materials
{
    public class Fire : Pixel
    {
        float Lifespan { get; set; }
        // Constructor
        public Fire(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifespan = .5f;
            Temperature = 2200f;
            Conductivity = .5f;
            Density = .0004f;
            Gravity = true;
            State = 3;
            Strength = 1000;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(600, typeof(Air));
        }
        public override void Update()
        {
            // Life
            Lifespan -= Canvas.Delta;
            if (Lifespan <= 0)
            {
                Pixel created = new Air(Location, Canvas);
                created.Velocity = Velocity;
                Canvas.Pixels[Flat(Coords.Y, Coords.X)] = created;
                return;
            }

            // Glow
            if (Canvas.Rand.Next(0, 3) == 0) { Color = ColorSchemes.GetColor(TypeId); }

            // Spreading
            foreach (Pixel neighbor in Neighbors)
            {
                if (Canvas.Rand.Next(0, 20) == -1 && Tags.Flammable.Contains(neighbor.GetType()))
                {
                    Pixel created = new Fire(neighbor.Location, Canvas);
                    created.Velocity = neighbor.Velocity;
                    Canvas.Pixels[Flat(created.Coords.Y, created.Coords.X)] = created;
                }
            }

            // Base
            base.Update();
        }
    }
}
