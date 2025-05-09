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
            Melting = new Transformation(999999, typeof(Ceramic));
            Solidifying = new Transformation(-999999, typeof(Ceramic));
            Gravity = false;
        }
    }
}
