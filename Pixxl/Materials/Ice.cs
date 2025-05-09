using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Ice : Pixel
    {
        // Constructor
        public Ice(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Temperature = 0f;
            Conductivity = 2.18f;
            Density = .917f;
            State = 0;
            Strength = 30;
            Melting = new Transformation(32, typeof(Water));
            Solidifying = new Transformation(-999999, typeof(Ice));
            Gravity = false;
        }
    }
}
