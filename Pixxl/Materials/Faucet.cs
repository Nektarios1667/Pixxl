using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pixxl.Materials
{
    public class Faucet : Pixel
    {
        // Constructor
        private int cycle { get; set; }
        public Faucet(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.8f;
            State = 0;
            Strength = 600;
            Melting = new Transformation(999999, typeof(Faucet));
            Solidifying = new Transformation(-999999, typeof(Faucet));
            Gravity = false;
            cycle = 0;
        }

        public override void Update()
        {
            base.Update();

            // Water
            Pixel? down = Neighbors[4];
            if (down != null && down.Type == "Air" && cycle == 0)
            {
                down.Skip = true;
                Canvas.Pixels[down.Index] = new Water(down.Location, Canvas);
            }
            cycle = (cycle + 1) % 2;
        }
    }
}
