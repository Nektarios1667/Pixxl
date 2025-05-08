using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pixxl.Materials
{
    public class Flare : Fuel
    {
        // Constructor
        public Flare(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 25f;
            Ashes = false;
            Conductivity = .02f;
            Density = .88f;
            State = 2;
            Strength = 70;
            Melting = new Transformation(999999, typeof(Flare));
            Solidifying = new Transformation(-999999, typeof(Flare));
        }

        public override void Update()
        {
            base.Update();

            // Burned out
            if (Burned >= Lifetime)
            {
                Canvas.Pixels[Index] = new FlareSmoke(Location, Canvas);
                Canvas.Pixels[Index].Skip = true;
                return;
            }

            // Burn
            Burned += Canvas.Delta;
            // Ontop
            Pixel? above = Neighbors[0];
            Pixel? aboveLeft = Neighbors[7];
            Pixel? aboveRight = Neighbors[1];
            // Above
            if (above != null && above.Type == "Air")
            {
                Canvas.Pixels[above.GetIndex()] = new FlareSmoke(above.Location, Canvas);
            }
            // Above left
            if (aboveLeft != null && aboveLeft.Type == "Air" && Canvas.ChancePerSecond(5))
            {
                Canvas.Pixels[aboveLeft.GetIndex()] = new FlareSmoke(aboveLeft.Location, Canvas);
            }
            // Above left
            if (aboveRight != null && aboveRight.Type == "Air" && Canvas.ChancePerSecond(5))
            {
                Canvas.Pixels[aboveRight.GetIndex()] = new FlareSmoke(aboveRight.Location, Canvas);
            }
        }
    }
}
