using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Consts = Pixxl.Constants;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pixxl.Materials
{
    public class Cloud : Pixel
    {
        // Constructor
        public Cloud(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .32f;
            Density = .0012f;
            State = 3;
            Strength = 80;
            Melting = new Transformation(350, typeof(Steam));
            Solidifying = new Transformation(32, typeof(Snow));
        }
        public override void Update()
        {
            base.Update();

            // Storming
            foreach (Pixel? neighbor in Neighbors)
            {
                if (neighbor != null && neighbor.Type == "Storm" && Canvas.Rand.Next(0, 40) == 0) {
                    Canvas.Pixels[Index] = new Storm(Location, Canvas);
                    return;
                }
            }
        }
        public override bool Movements() { return false; }
        public override void FluidSpread()
        {
            if (Canvas.Rand.Next(0, 20) != 0) { return; }

            int side = Canvas.Rand.Next(0, 2) == 0 ? -Consts.Screen.PixelSize : Consts.Screen.PixelSize;
            Xna.Vector2 next = new(Location.X + side, Location.Y);
            Pixel? target = Find(next, 'l');
            if (target != null && (target.Type == "Air" || target.Type == Type))
            {
                Location = Swap(Location, next);
                UpdatePositions();
            }
        }
    }
}
