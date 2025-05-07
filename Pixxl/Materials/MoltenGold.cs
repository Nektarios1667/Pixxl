using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MoltenGold : Pixel
    {
        // Constructor
        public MoltenGold(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 3000;
            Conductivity = 100f;
            Density = 17.3f;
            State = 3;
            Strength = 75;
            Melting = new Transformation(999999, typeof(MoltenGold));
            Solidifying = new Transformation(1700, typeof(Gold));
        }
    }
}
