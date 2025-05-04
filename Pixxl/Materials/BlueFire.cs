using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Pixxl;

namespace Pixxl.Materials
{
    public class BlueFire : Pixel
    {
        float Lifespan { get; set; }
        // Constructor
        public BlueFire(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifespan = .3f;
            Temperature = 9400f;
            Conductivity = .2f;
            Density = .0005f;
            Gravity = true;
            State = 4;
            Strength = 1000;
            Melting = new Transformation(999999, typeof(BlueFire));
            Solidifying = new Transformation(7200, typeof(Fire));
        }
        public override void Update()
        {
            // Base
            base.Update();

            // Life
            Lifespan -= Canvas.Delta;
            if (Lifespan <= 0)
            {
                Pixel created = new Air(Location, Canvas);
                Canvas.Pixels[Index] = created;
                return;
            }

            // Glow
            if (Canvas.Rand.Next(0, 3) == 0) { Color = ColorSchemes.GetColor(TypeId); }

            // Spreading
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && Canvas.Rand.Next(0, 5) == 0 && neighbor is IBurnable burnable)
                {
                    burnable.Ignite();
                }
            }
        }
    }
}
