using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Coal : Fuel
    {
        // Constructor
        public Coal(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Lifetime = 10f;
            Conductivity = .3f;
            Density = 1.8f;
            Strength = 50;
            Melting = new Transformation(Int32.MaxValue, typeof(Coal));
            Solidifying = new Transformation(Int32.MinValue, typeof(Coal));
        }
    }
}
