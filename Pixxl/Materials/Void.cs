using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Void : Pixel
    {
        // Constructor
        public Void(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 0f;
            Density = 1000f;
            State = 0;
            Strength = 999999;
            Melting = new Transformation(999999, typeof(Void));
            Solidifying = new Transformation(-999999, typeof(Void));
            Gravity = false;
            Color = ColorSchemes.Void();
        }

        public override void Update()
        {
            base.Update();

            // Destrory
            foreach (Pixel neighbor in Neighbors)
            {
                if (neighbor.Type != "Air" && neighbor.Type != "Void")
                {
                    Canvas.Pixels[Flat(Coord(neighbor.Location))] = new Air(neighbor.Snapped, Canvas);
                }
            }
        }
    }
}
