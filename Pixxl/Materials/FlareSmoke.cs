using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Consts = Pixxl.Constants;

namespace Pixxl.Materials
{
    public class FlareSmoke : Pixel
    {
        private float Stagnant { get; set; }
        // Constructor
        public FlareSmoke(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 800f;
            Conductivity = .02f;
            Density = .00042f;
            State = 3;
            Strength = 90;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(400, typeof(Air));
            Stagnant = 0;
        }
        public override void Update()
        {
            // Skipped
            if (Skip) { Skip = false; return; }

            // Stagnant long enough to dissipate
            if (Stagnant > 2f) { Canvas.Pixels[Index] = new Air(Location, Canvas); return; }

            // Reset
            GetNeighbors();

            // Stagnant
            if (Location == Previous) { Stagnant += Canvas.Delta; }

            // Heat transfer
            if (!SkipHeat) { HeatTransfer(); }
            else { SkipHeat = false; }

            // Check changes for melting, evaporating, plasmifying, deplasmifying condensing, hardening
            StateCheck();

            // Final
            Previous = Location;
        }
    }
}
