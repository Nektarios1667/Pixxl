using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Consts = Pixxl.Constants;

namespace Pixxl.Materials
{
    public class Grass : Pixel
    {
        // Constructor
        public bool End { get; set; }
        public Grass(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            End = Canvas.Rand.Next(0, 2) == 0;
            Conductivity = .001f;
            Density = .6f;
            State = 1;
            Strength = 20;
            Melting = new Transformation(999999, typeof(Grass));
            Solidifying = new Transformation(-999999, typeof(Grass));
        }
        public override void Update()
        {
            base.Update();

            // Growing
            if (End) { return; }
            Pixel? above = Neighbors[0];
            if (above != null && Canvas.Rand.Next(0, 200) == 0 && above.Type == "Air") {
                Grass created = new Grass(above.Location, Canvas);
                if (Canvas.Rand.Next(0, 3) == 0) { created.End = true; }
                Canvas.Pixels[above.Index] = created;
            }
        }
    }
}
