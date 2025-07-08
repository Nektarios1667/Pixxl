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
            Solidifying = new Transformation(Int32.MinValue, typeof(Rubber));
            Gravity = false;
        }
    }
}
