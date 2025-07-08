using Xna = Microsoft.Xna.Framework;

namespace Pixxl.Materials
{
    public class Mercury : Pixel
    {
        // Constructor
        public Mercury(Xna.Vector2 location, Canvas canvas) : base(location, canvas)
        {
            // Constants
            Conductivity = 2.35f;
            Density = 13.5f;
            State = 3;
            Strength = 200;
            Melting = new Transformation(Int32.MaxValue, typeof(Mercury));
            Solidifying = new Transformation(-50, typeof(SolidMercury));
        }
    }
}
