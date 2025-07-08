using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Water : Pixel
    {
        // Constructor
        public Water(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .04f;
            Density = 1f;
            State = 3;
            Strength = Int32.MaxValue;
            Melting = new Transformation(212, typeof(Steam));
            Solidifying = new Transformation(32, typeof(Ice));
        }
    }
}
