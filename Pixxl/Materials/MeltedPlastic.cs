using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MeltedPlastic : Pixel
    {
        // Constructor
        public MeltedPlastic(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .05f;
            Density = .96f;
            State = 3;
            Strength = 100;
            Melting = new Transformation(999999, typeof(MeltedPlastic));
            Solidifying = new Transformation(250, typeof(Plastic));
        }
    }
}
