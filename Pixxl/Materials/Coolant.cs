using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Coolant : Pixel
    {
        // Constructor
        public Coolant(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 110f;
            Density = 1.1f;
            State = 3;
            Strength = 600;
            Melting = new Transformation(700, typeof(CoolantVapor));
            Solidifying = new Transformation(-9500, typeof(Coolant));
        }
    }
}
