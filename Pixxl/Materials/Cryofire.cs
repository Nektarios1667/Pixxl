using System;
using Xna = Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using Pixxl;

namespace Pixxl.Materials
{
    public class Cryofire : Fire
    {
        // Constructor
        public Cryofire(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .5f;
            Lifespan = .7f;
            Temperature = -400f;
            Melting = new Transformation(0, typeof(Fire));
            Solidifying = new Transformation(-500, typeof(Ice));
        }
        public override void Spread() {}
    }
}
