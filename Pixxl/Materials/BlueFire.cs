using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Pixxl;

namespace Pixxl.Materials
{
    public class BlueFire : Fire
    {
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
        public override void Spread()
        {
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && Canvas.ChancePerSecond(3) && (neighbor is IIgnitable ignitable))
                {
                    ignitable.Ignite();
                }
            }
        }
    }
}
