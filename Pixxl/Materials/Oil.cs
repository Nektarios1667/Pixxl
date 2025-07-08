using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Oil : Fuel
    {
        // Constructor
        public Oil(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 15;
            Conductivity = .15f;
            Density = .85f;
            State = 3;
            Strength = 400;
            Melting = new Transformation(9200, typeof(Plasma));
            Solidifying = new Transformation(Int32.MinValue, typeof(Oil));
        }
    }
}
