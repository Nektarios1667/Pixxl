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
            Lifespan = .3f;
            Temperature = 800f;
            Conductivity = 1f;
            Density = .0004f;
            Gravity = true;
            State = 3;
            Strength = 1000;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(600, typeof(Air));
            Color = ColorSchemes.Fire();
        }
        public override void Update()
        {
            base.Update();

            // Life
            Lifespan -= Canvas.Delta;
            if (Lifespan <= 0)
            {
                Pixel created = new Air(Location, Canvas);
                created.Velocity = Velocity;
                Canvas.Pixels[(int)Coords.Y, (int)Coords.X] = created;
                return;
            }

            // Glow
            if (Canvas.Rand.Next(0, 3) == 0) { Color = ColorSchemes.Fire(); }

            // Spreading
            bool spread = false;
            foreach (Pixel neighbor in Neighbors)
            {
                if (Canvas.Rand.Next(0, 20) == -1 && Tags.Flammable.Contains(neighbor.GetType()))
                {
                    Pixel created = new Fire(neighbor.Location, Canvas);
                    created.Velocity = neighbor.Velocity;
                    Canvas.Pixels[(int)created.Coords.Y, (int)created.Coords.X] = created;
                    spread = true;
                }
            }
        }
    }
}
