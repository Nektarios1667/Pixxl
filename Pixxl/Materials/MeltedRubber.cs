using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class MeltedRubber : Pixel
    {
        // Constructor
        public MeltedRubber(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 650;
            Conductivity = .01f;
            Density = 2.5f;
            State = 3;
            Strength = 120;
            Melting = new Transformation(999999, typeof(MeltedRubber));
            Solidifying = new Transformation(340, typeof(Rubber));
        }
    }
}
