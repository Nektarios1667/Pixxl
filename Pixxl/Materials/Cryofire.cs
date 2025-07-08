using Xna = Microsoft.Xna.Framework;

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
            Melting = new Transformation(Int32.MaxValue, typeof(Cryofire));
            Solidifying = new Transformation(Int32.MinValue, typeof(Cryofire));
        }
        public override void Spread() { }
    }
}
