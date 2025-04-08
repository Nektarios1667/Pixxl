using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pixxl.Materials
{
    public class Flare : Fueling
    {
        // Constructor
        public Flare(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Fuel = 25;
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
            if (Burned >= Fuel)
            {
                Canvas.Pixels[Index] = new FlareSmoke(Location, Canvas);
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
                Canvas.Pixels[above.GetIndex()] = new FlareSmoke(above.Location, Canvas); above.Skip = true;
            }
            // Above left
            if (aboveLeft != null && aboveLeft.Type == "Air" && Canvas.Rand.Next(0, 10) == 0)
            {
                Canvas.Pixels[aboveLeft.GetIndex()] = new FlareSmoke(aboveLeft.Location, Canvas); aboveLeft.Skip = true;
            }
            // Above left
            if (aboveRight != null && aboveRight.Type == "Air" && Canvas.Rand.Next(0, 10) == 0)
            {
                Canvas.Pixels[aboveRight.GetIndex()] = new FlareSmoke(aboveRight.Location, Canvas); aboveRight.Skip = true;
            }
        }
    }
}
