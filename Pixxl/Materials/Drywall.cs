using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Drywall : Fuel
    {
        // Constructor
        public Drywall(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 3;
            Conductivity = .1f;
            Density = .3f;
            State = 1;
            Strength = 80;
            Melting = new Transformation(Int32.MaxValue, typeof(Drywall));
            Solidifying = new Transformation(Int32.MinValue, typeof(Drywall));
            Gravity = false;
        }
    }
}
