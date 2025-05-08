using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Pixxl;

namespace Pixxl.Materials
{
    public class Fire : Pixel
    {
        public float Lifespan { get; set; }
        // Constructor
        public Fire(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifespan = .5f;
            Temperature = 2200f;
            Conductivity = .5f;
            Density = .0004f;
            Gravity = true;
            State = 4;
            Strength = 1000;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(40, typeof(Air));
        }
        public override void Update()
        {
            // Base
            base.Update();

            // Life
            TickLife();

            // Glow
            Glow();

            // Spreading
            Spread();
        }

        public virtual void TickLife()
        {
            Lifespan -= Canvas.Delta;
            if (Lifespan <= 0)
            {
                UpdatePositions();
                Canvas.Pixels[Index] = new Air(Location, Canvas);
                Canvas.Pixels[Index].Temperature = Math.Max(Temperature / 5, Constants.Game.RoomTemp);
                Skip = true;
                return;
            }
        }

        public virtual void Glow()
        {
            if (Canvas.Rand.Next(0, 3) == 0) { Color = ColorSchemes.GetColor(TypeId); }
        }

        public virtual void Spread()
        {
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && Canvas.Rand.Next(0, 20) == 0 && (neighbor is IIgnitable ignitable))
                {
                    ignitable.Ignite();
                }
            }
        }
    }
}
