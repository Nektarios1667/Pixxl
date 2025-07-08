using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Ceramic : Pixel
    {
        // Constructor
        public Ceramic(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .008f;
            Density = 4f;
            State = 0;
            Strength = 150;
            Melting = new Transformation(Int32.MaxValue, typeof(Ceramic));
            Solidifying = new Transformation(Int32.MinValue, typeof(Ceramic));
            Gravity = false;
        }
    }
}
