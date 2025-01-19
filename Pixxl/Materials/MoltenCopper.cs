using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MoltenCopper : Pixel
    {
        // Constructor
        public MoltenCopper(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 6f;
            Density = 6.1f;
            State = 3;
            Strength = 150;
            Melting = new Transformation(999999, typeof(MoltenCopper));
            Solidifying = new Transformation(2000, typeof(Copper));
        }
    }
}
