using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Pixxl.Materials
{
    public class Torch : Pixel
    {
        // Constructor
        public Torch(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = 2.8f;
            State = 0;
            Strength = 600;
            Melting = new Transformation(999999, typeof(Torch));
            Solidifying = new Transformation(-999999, typeof(Torch));
            Gravity = false;
        }

        public override void Update()
        {
            base.Update();

            // Water
            Xna.Vector2 spawn = new(Location.X, Location.Y - Constants.Screen.PixelSize);
            int idx = Flat(Coord(spawn));
            if (Canvas.Pixels[idx].Type == "Air")
            {
                Canvas.Pixels[idx].Ignore = true;
                Canvas.Pixels[idx] = new Fire(spawn, Canvas);
            }
        }
    }
}
