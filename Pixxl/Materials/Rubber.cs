using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Rubber : Pixel
    {
        // Constructor
        public Rubber(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = .03f;
            Density = 2.3f;
            State = 0;
            Strength = 125;
            Melting = new Transformation(380, typeof(MeltedRubber));
            Solidifying = new Transformation(-999999, typeof(Rubber));
            Gravity = false;
        }
    }
}
