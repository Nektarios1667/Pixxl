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
            Conductivity = 4f;
            Lifespan = .7f;
            Temperature = -1000f;
            Melting = new Transformation(999999, typeof(Cryofire));
            Solidifying = new Transformation(-999999, typeof(Cryofire));
        }
        public override void Spread() {}
    }
}
